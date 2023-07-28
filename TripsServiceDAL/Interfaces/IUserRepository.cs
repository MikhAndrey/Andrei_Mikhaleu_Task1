using TripsServiceDAL.Entities;

namespace TripsServiceDAL.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
	Task<User?> GetByUsernameAsync(string username);
	Task<User?> GetByEmailAsync(string email);
	IQueryable<User> GetAllWithRoles();
}
