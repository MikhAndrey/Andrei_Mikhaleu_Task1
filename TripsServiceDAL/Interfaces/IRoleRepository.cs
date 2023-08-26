using TripsServiceDAL.Entities;

namespace TripsServiceDAL.Interfaces;

public interface IRoleRepository
{
    Role? GetByName(string roleName);
}
