using TripsServiceDAL.Entities;
using TripsServiceDAL.Infrastructure;
using TripsServiceDAL.Interfaces;

namespace TripsServiceDAL.Repos;

public class RoleRepository : EFGenericRepository<Role>, IRoleRepository
{
    public RoleRepository(TripsDBContext context) : base(context)
    {
    }

    public Role? GetByName(string roleName)
    {
        return _dbSet.FirstOrDefault(role => role.Name == roleName);
    }
}
