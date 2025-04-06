using CNCARTOON.DataAccess.Context;
using CNCARTOON.DataAccess.IRepository;

namespace CNCARTOON.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IUserRepository UserRepository { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            UserRepository = new UserRepository(_context);
        }
        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
