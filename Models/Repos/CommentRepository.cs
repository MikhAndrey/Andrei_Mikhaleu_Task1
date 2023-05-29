using Andrei_Mikhaleu_Task1.Models.Entities;
using Andrei_Mikhaleu_Task1.Models.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace Andrei_Mikhaleu_Task1.Models.Repos
{
    public class CommentRepository
    {
        private readonly TripsDBContext _context;

        public CommentRepository(TripsDBContext context)
        {
            _context = context;
        }

        public async Task Add(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Comment comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Comment comment)
        {
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<Comment> GetById(int id)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Trip)
                .FirstOrDefaultAsync(c => c.CommentId == id);
        }
        public async Task<List<Comment>> GetCommentsByTripId(int tripId)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Trip)
                .Where(c => c.TripId == tripId)
                .ToListAsync();
        }
    }
}
