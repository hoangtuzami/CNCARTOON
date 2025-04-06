using CNCARTOON.Models.DTO.Auth;
using CNCARTOON.Models.DTO.Others;
using CNCARTOON.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace CNCARTOON.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("sign-up")]
        public async Task<ActionResult<ResponseDTO>> SignUpCustomer([FromBody] SignUpUserDTO signUpUserDTO)
        {
            var responseDto = await _authService.SignUpCustomerAsync(signUpUserDTO);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPost("sign-in")]
        public async Task<ActionResult<ResponseDTO>> SignIn([FromBody] SignInDTO signInDTO)
        {
            var responseDto = await _authService.SignInAsync(signInDTO);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPost("logout")]
        public async Task<ActionResult<ResponseDTO>> Logout([FromBody] LogoutDTO logout)
        {
            var responseDto = await _authService.LogoutAsync(logout);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
    }
}
