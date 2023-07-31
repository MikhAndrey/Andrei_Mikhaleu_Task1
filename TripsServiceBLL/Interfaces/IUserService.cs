using TripsServiceBLL.DTO.Users;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Interfaces;

public interface IUserService
{
	Task<User?> GetUserForLoginAsync(UserLoginDTO user);
	Task AddAsync(User user);
	bool Exists(int id);
	Task TryToRegisterNewUserAsync(UserSignupDTO user);
	Task<string> GetJWTTokenAsync(UserLoginDTO user);
	int GetCurrentUserId();
	string? GetCurrentUserName();
	string? GetCurrentUserRole();
	IEnumerable<UserListDTO> GetAllNamesWithRoles();
}
