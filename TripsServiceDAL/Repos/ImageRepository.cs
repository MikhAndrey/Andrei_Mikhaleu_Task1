using Microsoft.EntityFrameworkCore;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Infrastructure;
using TripsServiceDAL.Interfaces;

namespace TripsServiceDAL.Repos
{
    public class ImageRepository : EFGenericRepository<Image>, IImageRepository
    {
        public ImageRepository(TripsDBContext context) : base(context) { }

        public new async Task<Image?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(i => i.Trip)
                .FirstOrDefaultAsync(i => i.ImageId == id);
        }

        public IQueryable<Image> GetImagesByTripId(int tripId)
        {
            return _dbSet
                .Include(i => i.Trip)
                .Where(i => i.TripId == tripId);
        }
    }
}
