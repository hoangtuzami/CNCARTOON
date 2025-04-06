using CNCARTOON.DataAccess.IRepository;
using CNCARTOON.Models.Domain;
using CNCARTOON.Models.DTO.Auth;
using CNCARTOON.Models.DTO.Others;
using CNCARTOON.Services.IServices;
using CNCARTOON.Utility.Constants;
using Microsoft.AspNetCore.Identity;

namespace CNCARTOON.Services.Services
{
    public class AuthSerivce : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public AuthSerivce
        (
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUnitOfWork unitOfWork
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }

        public Task<ResponseDTO> SignInAsync(SignInDTO signIpDTO)
        {
            throw new NotImplementedException();
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
                        Message = "Email is already in use"
                    };
                }

                var newCustomer = new ApplicationUser()
                {
                    UserName = signUpUserDTO.UserName,
                    Email = signUpUserDTO.Email,
                    FullName = signUpUserDTO.FullName,
                    EmailConfirmed = true
                };

                var reuslt = await _userManager.CreateAsync(newCustomer, signUpUserDTO.Password);
                if (!reuslt.Succeeded)
                {
                    var errors = string.Join(" ", reuslt.Errors.Select(e => e.Description));
                    return new ResponseDTO
                    {
                        IsSuccess = false,
                        StatusCode = 400,
                        Message = $"User creation failed: {errors}"

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
                        Message = $"User role assignment failed: {errors}"
                    };
                }
                await _unitOfWork.SaveAsync();

                return new ResponseDTO
                {
                    IsSuccess = true,
                    StatusCode = 201,
                    Message = "User registered successfully",
                    Result = new { newCustomer.Id, newCustomer.Email}
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = $"Error: {ex.Message}"
                };
            }
        }
    }
}
