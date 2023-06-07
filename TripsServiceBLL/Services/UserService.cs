using TripsServiceBLL.DTO;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Infrastructure;

namespace TripsServiceBLL.Services
{
    public class UserService
    {
        private readonly UnitOfWork _unitOfWork;

        public UserService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User?> GetByUserNameAsync(string? userName)
        {
            return await _unitOfWork.Users.GetByUsernameAsync(userName);
        }

        public async Task<bool> UserExists(UserLoginDTO user)
        {
            User? userFromDB = await GetByUserNameAsync(user.UserName);
            return userFromDB != null && userFromDB.Password == user.Password;
        }

        public async Task AddAsync(User user)
        {
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.Save();
        }
    }
}
