using CNCARTOON.DataAccess.Context;
using CNCARTOON.DataAccess.IRepository;
using CNCARTOON.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CNCARTOON.DataAccess.Repository
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<ApplicationUser> GetByIdAsync(string id)
        {
            return await _context.ApplicationUsers.FirstOrDefaultAsync(us => us.Id == id) ?? throw new InvalidOperationException("User not found");
        }


        public void Update(ApplicationUser user)
        {
            _context.ApplicationUsers.Update(user);
        }

        public void UpdateRange(IEnumerable<ApplicationUser> users)
        {
            _context.ApplicationUsers.UpdateRange(users);
        }

        public async Task<ApplicationUser> AddAsync(ApplicationUser user)
        {
            var entity = await _context.ApplicationUsers.AddAsync(user);
            return entity.Entity;
        }
    }
}
