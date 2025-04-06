using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNCARTOON.Services.IServices
{
    public interface IRedisService
    {
        Task<bool> StoreStringAsync(string key, string value, TimeSpan? expiry = null);
        Task<string> RetrieveStringAsync(string key);
        Task<bool> DeleteStringAsync(string key);
    }
}
