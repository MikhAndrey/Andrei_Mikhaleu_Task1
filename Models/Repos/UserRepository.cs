using Andrei_Mikhaleu_Task1.Models.Entities;
using Andrei_Mikhaleu_Task1.Models.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace Andrei_Mikhaleu_Task1.Models.Repos
{
    public class UserRepository
    {
        private readonly TripsDBContext _context;

        public UserRepository(TripsDBContext context)
        {
            _context = context;
        }

        public async Task Add(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task Update(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var user = await GetById(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetById(int id)
        {
            return await _context.Users
                .Include(u => u.Trips)
                .Include(u => u.Comments)
                .FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<User> GetByUsername(string username)
        {
            return await _context.Users
                .Include(u => u.Trips)
                .Include(u => u.Comments)
                .FirstOrDefaultAsync(u => u.UserName == username);
        }
    }
}
