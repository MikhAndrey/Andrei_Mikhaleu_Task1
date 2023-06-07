using Microsoft.EntityFrameworkCore;
using TripsServiceDAL.Infrastructure;
using TripsServiceDAL.Entities;

namespace TripsServiceDAL.Repos
{
    public class UserRepository : EFGenericRepository<User>
    {
        public UserRepository(TripsDBContext context) : base(context) { }

        public async new Task<User?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(u => u.Trips)
                .Include(u => u.Comments)
                .FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<User?> GetByUsernameAsync(string? username)
        {
            return await _dbSet
                .Include(u => u.Trips)
                .Include(u => u.Comments)
                .FirstOrDefaultAsync(u => u.UserName == username);
        }
    }
}
