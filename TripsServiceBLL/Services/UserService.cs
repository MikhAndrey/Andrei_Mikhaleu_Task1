using TripsServiceBLL.DTO.Users;
using TripsServiceDAL.Entities;
using TripsServiceBLL.Infrastructure;
using TripsServiceDAL.Interfaces;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Utils;

namespace TripsServiceBLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User?> GetByUserNameAsync(string userName)
        {
            return await _unitOfWork.Users.GetByUsernameAsync(userName);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _unitOfWork.Users.GetByEmailAsync(email);
        }

        public async Task<bool> UserExistsAsync(UserLoginDTO user)
        {
            User? userFromDB = await GetByUserNameAsync(user.UserName);
            return userFromDB != null && userFromDB.Password == Encryptor.Encrypt(user.Password);
        }

        public async Task AddAsync(User user)
        {
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveAsync();
        }

        public async Task TryToRegisterNewUserAsync(UserSignupDTO user)
        {
			User? existingUser = await GetByUserNameAsync(user.UserName);
			if (existingUser != null)
				throw new ValidationException("This username is already taken", "UserName");
			existingUser = await GetByEmailAsync(user.Email);
			if (existingUser != null)
				throw new ValidationException("This email is already taken", "Email");
			User newUser = new()
			{
				UserName = user.UserName,
				Password = Encryptor.Encrypt(user.Password),
				Email = user.Email
			};

			await AddAsync(newUser);
		}
    }
}
