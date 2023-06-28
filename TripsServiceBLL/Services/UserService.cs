using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TripsServiceBLL.DTO.Users;
using TripsServiceBLL.Infrastructure.Exceptions;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Utils;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Interfaces;

namespace TripsServiceBLL.Services
{
	public class UserService : IUserService
	{
		private readonly IUnitOfWork _unitOfWork;

		private readonly IMapper _mapper;

		public UserService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<User?> GetByUserNameAsync(string userName)
		{
			return await _unitOfWork.Users.GetByUsernameAsync(userName);
		}

		public async Task<User?> GetByEmailAsync(string email)
		{
			return await _unitOfWork.Users.GetByEmailAsync(email);
		}

		public async Task<int?> GetUserIdForLoginAsync(UserLoginDTO user)
		{
			User? userFromDB = await GetByUserNameAsync(user.UserName);
			return (userFromDB != null && userFromDB.Password == UtilEncryptor.Encrypt(user.Password))
				? userFromDB.Id
				: null;
		}

		public async Task AddAsync(User user)
		{
			await _unitOfWork.Users.AddAsync(user);
			await _unitOfWork.SaveAsync();
		}

		public bool Exists(int id)
		{
			return _unitOfWork.Users.Exists(id);
		}

		public async Task TryToRegisterNewUserAsync(UserSignupDTO user)
		{
			User? existingUser = await GetByUserNameAsync(user.UserName);
			if (existingUser != null)
			{
				throw new ValidationException(UtilConstants.GetExistingCredentialMessage("username"), "UserName");
			}

			existingUser = await GetByEmailAsync(user.Email);
			if (existingUser != null)
			{
				throw new ValidationException(UtilConstants.GetExistingCredentialMessage("email"), "Email");
			}

			User userToAdd = _mapper.Map<User>(user);
			await AddAsync(userToAdd);
		}

		public async Task<string> GetJWTTokenAsync(UserLoginDTO user)
		{
			int? idOfUserFromDb = await GetUserIdForLoginAsync(user);
			if (idOfUserFromDb != null)
			{
				DateTime jwtExpiresUTC = user.RememberMe ? DateTime.UtcNow.AddDays(UtilConstants.AuthorizationExpirationInDays)
					: DateTime.UtcNow.AddHours(UtilConstants.JwtExpirationInHours);
				JwtSecurityTokenHandler tokenHandler = new();
				byte[] key = Encoding.ASCII.GetBytes(UtilConstants.JwtKey);
				SecurityTokenDescriptor tokenDescriptor = new()
				{
					Subject = new ClaimsIdentity(new Claim[]
					{
							new Claim(ClaimTypes.Name, user.UserName),
							new Claim (UtilConstants.UserIdClaimName, idOfUserFromDb.ToString())
					}),
					Audience = UtilConstants.JwtIssuer,
					Issuer = UtilConstants.JwtIssuer,
					Expires = jwtExpiresUTC,
					SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
				};
				SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
				return tokenHandler.WriteToken(token);
			}
			else
			{
				throw new ValidationException(UtilConstants.InvalidCredentialsMessage, "");
			}
		}
	}
}
