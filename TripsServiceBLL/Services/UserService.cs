using TripsServiceBLL.DTO.Users;
using TripsServiceDAL.Entities;
using TripsServiceBLL.Infrastructure;
using TripsServiceDAL.Interfaces;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Utils;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

		public async Task<User?> GetByIdAsync(int id)
		{
			return await _unitOfWork.Users.GetByIdAsync(id);
		}

		public async Task<int?> GetUserIdForLoginAsync(UserLoginDTO user)
		{
			User? userFromDB = await GetByUserNameAsync(user.UserName);
			return (userFromDB != null && userFromDB.Password == Encryptor.Encrypt(user.Password)) ? userFromDB.UserId : null;
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

		public async Task<string> GetJWTTokenAsync(UserLoginDTO user)
		{
			int? idOfUserFromDb = await GetUserIdForLoginAsync(user);
			if (idOfUserFromDb != null)
			{
				DateTime jwtExpiresUTC = user.RememberMe ? DateTime.UtcNow.AddDays(7) : DateTime.UtcNow.AddHours(1);
				JwtSecurityTokenHandler tokenHandler = new();
				byte[] key = Encoding.ASCII.GetBytes(Constants.JwtKey);
				SecurityTokenDescriptor tokenDescriptor = new()
				{
					Subject = new ClaimsIdentity(new Claim[]
					{
							new Claim(ClaimTypes.Name, user.UserName),
							new Claim ("userId", idOfUserFromDb.ToString())
					}),
					Audience = Constants.JwtIssuer,
					Issuer = Constants.JwtIssuer,
					Expires = jwtExpiresUTC,
					SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
				};
				SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
				return tokenHandler.WriteToken(token);
			}
			else
				throw new ValidationException("Invalid credentials. Please, try again.", "");
		}
	}
}
