using Microsoft.EntityFrameworkCore;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Infrastructure;
using TripsServiceDAL.Interfaces;

namespace TripsServiceDAL.Repos
{
    public class UserRepository : EFGenericRepository<User>, IUserRepository
    {
        public UserRepository(TripsDBContext context) : base(context) { }

        public new async Task<User?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(u => u.Trips)
                .Include(u => u.Comments)
                .FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _dbSet
                .Include(u => u.Trips)
                .Include(u => u.Comments)
                .FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .Include(u => u.Trips)
                .Include(u => u.Comments)
                .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
