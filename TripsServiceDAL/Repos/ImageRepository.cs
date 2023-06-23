using TripsServiceDAL.Entities;
using TripsServiceDAL.Infrastructure;
using TripsServiceDAL.Interfaces;

namespace TripsServiceDAL.Repos
{
    public class ImageRepository : EFGenericRepository<Image>, IImageRepository
    {
        public ImageRepository(TripsDBContext context) : base(context) { }

    }
}
