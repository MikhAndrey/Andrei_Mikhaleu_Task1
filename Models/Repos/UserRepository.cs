using Andrei_Mikhaleu_Task1.Models.Entities;
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

        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = GetById(id);
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public User GetById(int id)
        {
            return _context.Users
                .Include(u => u.Trips)
                .Include(u => u.Comments)
                .FirstOrDefault(u => u.UserId == id);
        }

        public User GetByUsername(string username)
        {
            return _context.Users
                .Include(u => u.Trips)
                .Include(u => u.Comments)
                .FirstOrDefault(u => u.UserName == username);
        }
    }
}
