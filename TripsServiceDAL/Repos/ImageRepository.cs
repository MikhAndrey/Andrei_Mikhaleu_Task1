using Microsoft.EntityFrameworkCore;
using TripsServiceDAL.Infrastructure;
using TripsServiceDAL.Entities;

namespace TripsServiceDAL.Repos
{
    public class ImageRepository : EFGenericRepository<Image>
    {
        public ImageRepository(TripsDBContext context) : base(context) { }

        public async new Task<Image?> GetByIdAsync(int id)
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
