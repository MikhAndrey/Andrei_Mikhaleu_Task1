using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using TripsServiceBLL.DTO.Users;
using TripsServiceBLL.Infrastructure.Exceptions;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Utils;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Interfaces;

namespace TripsServiceBLL.Services;

public class UserService : IUserService
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	private readonly IMapper _mapper;

	private readonly IUnitOfWork _unitOfWork;

	public UserService(
		IUnitOfWork unitOfWork,
		IMapper mapper,
		IHttpContextAccessor httpContextAccessor)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_httpContextAccessor = httpContextAccessor;
	}

	public int GetCurrentUserId()
	{
		int userId = int.Parse(_httpContextAccessor.HttpContext.User.Claims
			.FirstOrDefault(c => c.Type == UtilConstants.UserIdClaimName)?.Value);
		_unitOfWork.Users.ThrowErrorIfNotExists(userId);
		return userId;
	}
	
	public string? GetCurrentUserName()
	{
		return _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
	}

	public async Task<User?> GetUserForLoginAsync(UserLoginDTO user)
	{
		User? userFromDB = await _unitOfWork.Users.GetByUsernameAsync(user.UserName);
		return userFromDB != null && userFromDB.Password == UtilEncryptor.Encrypt(user.Password)
			? userFromDB
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
		User? existingUser = await _unitOfWork.Users.GetByUsernameAsync(user.UserName);
		if (existingUser != null)
		{
			throw new ValidationException(UtilConstants.GetExistingCredentialMessage("username"), "UserName");
		}

		existingUser = await _unitOfWork.Users.GetByEmailAsync(user.Email);
		if (existingUser != null)
		{
			throw new ValidationException(UtilConstants.GetExistingCredentialMessage("email"), "Email");
		}

		User userToAdd = _mapper.Map<User>(user);
		await AddAsync(userToAdd);
	}

	public async Task<string> GetJWTTokenAsync(UserLoginDTO user)
	{
		User? UserFromDb = await GetUserForLoginAsync(user);
		if (UserFromDb != null)
		{
			DateTime jwtExpiresUTC = user.RememberMe
				? DateTime.UtcNow.AddDays(UtilConstants.AuthorizationExpirationInDays)
				: DateTime.UtcNow.AddHours(UtilConstants.JwtExpirationInHours);
			JwtSecurityTokenHandler tokenHandler = new();
			byte[] key = Encoding.ASCII.GetBytes(UtilConstants.JwtKey);
			SecurityTokenDescriptor tokenDescriptor = new()
			{
				Subject = new ClaimsIdentity(new[]
				{
					new(ClaimTypes.Name, user.UserName),
					new Claim(UtilConstants.UserIdClaimName, UserFromDb.Id.ToString()),
					new Claim(ClaimTypes.Role, UserFromDb.Role.Name)
				}),
				Audience = UtilConstants.JwtIssuer,
				Issuer = UtilConstants.JwtIssuer,
				Expires = jwtExpiresUTC,
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
					SecurityAlgorithms.HmacSha256Signature)
			};
			SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		throw new ValidationException(UtilConstants.InvalidCredentialsMessage, "Model");
	}
}
