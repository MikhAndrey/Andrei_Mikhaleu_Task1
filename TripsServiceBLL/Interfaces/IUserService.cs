using TripsServiceBLL.DTO.Users;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Interfaces
{
	public interface IUserService
	{
		Task<User?> GetByUserNameAsync(string userName);

		Task<User?> GetByEmailAsync(string email);

		Task<User?> GetByIdAsync(int id);

		Task<int?> GetUserIdForLoginAsync(UserLoginDTO user);

		Task AddAsync(User user);

		Task TryToRegisterNewUserAsync(UserSignupDTO user);
	}
}
