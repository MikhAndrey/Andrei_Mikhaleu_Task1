using Andrei_Mikhaleu_Task1.Models.Entities;
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

        public void Add(Comment comment)
        {
            _context.Comments.Add(comment);
            _context.SaveChanges();
        }

        public void Update(Comment comment)
        {
            _context.Comments.Update(comment);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var comment = GetById(id);
            _context.Comments.Remove(comment);
            _context.SaveChanges();
        }

        public Comment GetById(int id)
        {
            return _context.Comments
                .Include(c => c.User)
                .Include(c => c.Trip)
                .FirstOrDefault(c => c.CommentId == id);
        }
        public List<Comment> GetCommentsByTripId(int tripId)
        {
            return _context.Comments
                .Include(c => c.User)
                .Include(c => c.Trip)
                .Where(c => c.TripId == tripId)
                .ToList();
        }
    }
}
