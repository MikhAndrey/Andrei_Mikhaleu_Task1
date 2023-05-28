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

        public void Add(Image image)
        {
            _context.Images.Add(image);
            _context.SaveChanges();
        }

        public void Update(Image image)
        {
            _context.Images.Update(image);
            _context.SaveChanges();
        }

        public void Delete(Image image)
        {
            _context.Images.Remove(image);
            _context.SaveChanges();
        }

        public Image GetById(int id)
        {
            return _context.Images
                .Include(i => i.Trip)
                .FirstOrDefault(i => i.ImageId == id);
        }

        public List<Image> GetImagesByTripId(int tripId)
        {
            return _context.Images
                .Include(i => i.Trip)
                .Where(i => i.TripId == tripId)
                .ToList();
        }
    }
}
