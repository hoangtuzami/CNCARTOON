using CNCARTOON.DataAccess.IRepository;
using CNCARTOON.Models.Domain;
using CNCARTOON.Models.DTO.Auth;
using CNCARTOON.Models.DTO.Others;
using CNCARTOON.Services.IServices;
using CNCARTOON.Utility.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace CNCARTOON.Services.Services
{
    public class AuthSerivce : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        public AuthSerivce
        (
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUnitOfWork unitOfWork,
            ITokenService tokenService,
            IConfiguration config
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }

        public async Task<ResponseDTO> LogoutAsync(LogoutDTO logout)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(logout.UserId);
                if (user == null)
                {
                    return new ResponseDTO
                    {
                        IsSuccess = false,
                        StatusCode = 400,
                        Message = "Không tìm thấy tài khoản"
                    };
                }

                await _tokenService.DeleteRefreshTokenAsync(logout.UserId);

                return new ResponseDTO
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Message = "Đăng xuất thành công"
                };
            }
            catch(Exception ex)
            {
                return new ResponseDTO
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = $"Lỗi: {ex.Message}"
                };
            }
        }

        public async Task<ResponseDTO> SignInAsync(SignInDTO signIpDTO)
        {
            try
            {
                var user = _userManager.FindByNameAsync(signIpDTO.UserName).Result;
                if (user == null)
                {
                    return new ResponseDTO
                    {
                        IsSuccess = false,
                        StatusCode = 400,
                        Message = "Không tìm thấy tên tài khoản"
                    };
                }
                var result = _userManager.CheckPasswordAsync(user, signIpDTO.Password).Result;
                if (!result)
                {
                    new ResponseDTO
                    {
                        IsSuccess = false,
                        StatusCode = 400,
                        Message = "Mật khẩu không chính xác"
                    };
                }

                if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.UtcNow)
                {
                    var timeRemaining = user.LockoutEnd.Value - DateTime.UtcNow;
                    return new ResponseDTO
                    {
                        IsSuccess = false,
                        StatusCode = 403,
                        Message = $"Tài khoản tạm thời bị khóa. Vui lòng thử lại sau {timeRemaining.Minutes} phút"
                    };
                }

                if (user.LockoutEnabled)
                {
                    return new ResponseDTO
                    {
                        IsSuccess = false,
                        StatusCode = 403,
                        Message = "Tài khoản đã bị khóa"
                    };
                }

                if(!user.EmailConfirmed)
                {
                    return new ResponseDTO
                    {
                        IsSuccess = false,
                        StatusCode = 403,
                        Message = "Email chưa được xác thực"
                    };
                }

                var accessToken = await _tokenService.GenerateJwtAccessTokenAsync(user);
                var refreshToken = await _tokenService.GenerateJwtRefreshTokenAsync(user);
                await _tokenService.StoreRefreshTokenAsync(user.Id, refreshToken);

                return new ResponseDTO
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Message = "Đăng nhập thành công",
                    Result = new SignResponseDTO()
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken
                    }
                };

            }
            catch (Exception ex)
            {
                return new ResponseDTO
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = $"Lỗi: {ex.Message}"
                };
            }
        }

        public async Task<ResponseDTO> SignUpCustomerAsync(SignUpUserDTO signUpUserDTO)
        {
            try
            {
                var existingEmail = await _userManager.FindByEmailAsync(signUpUserDTO.Email);
                if (existingEmail != null)
                {
                    return new ResponseDTO
                    {
                        IsSuccess = false,
                        StatusCode = 400,
                        Message = "Email đã được sử dụng"
                    };
                }

                var newCustomer = new ApplicationUser()
                {
                    UserName = signUpUserDTO.UserName,
                    Email = signUpUserDTO.Email,
                    FullName = signUpUserDTO.FullName,
                    EmailConfirmed = false,
                    LockoutEnabled = false
                };

                var reuslt = await _userManager.CreateAsync(newCustomer, signUpUserDTO.Password);
                if (!reuslt.Succeeded)
                {
                    var errors = string.Join(" ", reuslt.Errors.Select(e => e.Description));
                    return new ResponseDTO
                    {
                        IsSuccess = false,
                        StatusCode = 400,
                        Message = $"Tạo user thất bại: {errors}"

                    };
                }

                var roleExist = await _roleManager.RoleExistsAsync(StaticUserRoles.Customer);
                if (!roleExist)
                {
                    var role = new IdentityRole
                    {
                        Name = StaticUserRoles.Customer,
                        NormalizedName = StaticUserRoles.Customer.ToUpper()
                    };
                    await _roleManager.CreateAsync(role);
                }

                var addRoleResult = await _userManager.AddToRoleAsync(newCustomer, StaticUserRoles.Customer);
                if (!addRoleResult.Succeeded)
                {
                    var errors = string.Join(" ", addRoleResult.Errors.Select(e => e.Description));
                    return new ResponseDTO
                    {
                        IsSuccess = false,
                        StatusCode = 400,
                        Message = $"Tạo user role thất bại: {errors}"
                    };
                }
                await _unitOfWork.SaveAsync();

                return new ResponseDTO
                {
                    IsSuccess = true,
                    StatusCode = 201,
                    Message = "Tạo user mới thành công",
                    Result = new { newCustomer.Id, newCustomer.Email }
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = $"Lỗi: {ex.Message}"
                };
            }
        }

        
    }
}
