using Andrei_Mikhaleu_Task1.Models.Entities;
using Andrei_Mikhaleu_Task1.Models.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace Andrei_Mikhaleu_Task1.Models.Repos
{
    public class ImageRepository
    {
        private readonly TripsDBContext _context;

        public ImageRepository(TripsDBContext context)
        {
            _context = context;
        }

        public async Task Add(Image image)
        {
            await _context.Images.AddAsync(image);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Image image)
        {
            _context.Images.Update(image);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Image image)
        {
            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
        }

        public async Task<Image> GetById(int id)
        {
            return await _context.Images
                .Include(i => i.Trip)
                .FirstOrDefaultAsync(i => i.ImageId == id);
        }

        public async Task<List<Image>> GetImagesByTripId(int tripId)
        {
            return await _context.Images
                .Include(i => i.Trip)
                .Where(i => i.TripId == tripId)
                .ToListAsync();
        }
    }
}
