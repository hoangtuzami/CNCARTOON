using CNCARTOON.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNCARTOON.Services.IServices
{
    public interface ITokenService
    {
        Task<string> GenerateJwtAccessTokenAsync(ApplicationUser user);
        Task<string> GenerateJwtRefreshTokenAsync(ApplicationUser user);
        Task<bool> StoreRefreshTokenAsync(string userId, string refreshToken);
        Task<bool> DeleteRefreshTokenAsync(string userId);
    }
}
