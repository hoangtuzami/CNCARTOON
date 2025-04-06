using CNCARTOON.Models.Domain;

namespace CNCARTOON.DataAccess.IRepository
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        void Update(ApplicationUser user);
        void UpdateRange(IEnumerable<ApplicationUser> users);
        Task<ApplicationUser> GetByIdAsync(string id);
        Task<ApplicationUser> AddAsync(ApplicationUser user);
    }
}
