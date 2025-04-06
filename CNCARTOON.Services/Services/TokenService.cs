using CNCARTOON.Models.Domain;
using CNCARTOON.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CNCARTOON.Services.Services
{
    public class TokenService : ITokenService
    {
        private readonly IRedisService _redisService;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        public TokenService(
            IRedisService redisService,
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager
        )
        {
            _redisService = redisService;
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<bool> DeleteRefreshTokenAsync(string userId)
        {
            string redisKey = $"{userId}-refresh-token";
            var result = await _redisService.DeleteStringAsync(redisKey);
            return result;
        }

        // Tạo access token có thời gian sống là 60 phút
        public async Task<string> GenerateJwtAccessTokenAsync(ApplicationUser user)
        {
            var userRole = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("FullName", user.FullName)
            };

            foreach (var role in userRole)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenObject = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["JWT:Expire"])),
                signingCredentials: creds
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenObject);
            return accessToken;
        }


        // Tạo refresh token có thời gian sống là 3 ngày
        public async Task<string> GenerateJwtRefreshTokenAsync(ApplicationUser user)
        {

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenObject = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                notBefore: DateTime.Now,
                claims: claims,
                expires: DateTime.Now.AddDays(Convert.ToDouble(_configuration["JWT:RefreshExpire"])),
                signingCredentials: creds
            );

            var refreshToken = new JwtSecurityTokenHandler().WriteToken(tokenObject);

            return refreshToken;
        }

        public async Task<bool> StoreRefreshTokenAsync(string userId, string refreshToken)
        {
            string redisKey = $"{userId}-refresh-token";
            var result = await _redisService.StoreStringAsync(redisKey, refreshToken, TimeSpan.FromDays(Convert.ToDouble(_configuration["JWT:RefreshExpire"])));
            return result;
        }
    }
}
