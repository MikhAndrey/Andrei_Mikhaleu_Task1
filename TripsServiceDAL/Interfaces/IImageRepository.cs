using TripsServiceDAL.Entities;

namespace TripsServiceDAL.Interfaces
{
    public interface IImageRepository : IGenericRepository<Image>
    {
        IQueryable<Image> GetByTripId(int tripId);
    }
}
