using TripsServiceDAL.Entities;

namespace TripsServiceDAL.Interfaces
{
    public interface IDriverRepository : IGenericRepository<Driver>
    {
        IQueryable<Driver> GetAll();
    }
}
