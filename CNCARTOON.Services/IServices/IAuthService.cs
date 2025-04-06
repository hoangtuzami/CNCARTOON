using CNCARTOON.Models.DTO.Auth;
using CNCARTOON.Models.DTO.Others;

namespace CNCARTOON.Services.IServices
{
    public interface IAuthService
    {
        public Task<ResponseDTO> SignInAsync(SignInDTO signIpDTO);
        public Task<ResponseDTO> SignUpCustomerAsync(SignUpUserDTO signUpUserDTO);
    }
}
