using TripsServiceDAL.Entities;

namespace TripsServiceDAL.Interfaces
{
    public interface IImageRepository : IGenericRepository<Image>
    {
        IQueryable<Image> GetImagesByTripId(int tripId);
    }
}
