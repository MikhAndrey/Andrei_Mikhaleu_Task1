using TripsServiceBLL.DTO.Users;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Interfaces
{
	public interface IUserService
	{
		Task<User?> GetByUserNameAsync(string userName);

		Task<User?> GetByEmailAsync(string email);

		Task<int?> GetUserIdForLoginAsync(UserLoginDTO user);

		Task AddAsync(User user);

		bool Exists(int id);

		Task TryToRegisterNewUserAsync(UserSignupDTO user);

		Task<string> GetJWTTokenAsync(UserLoginDTO user);

		int GetCurrentUserId();
	}
}
