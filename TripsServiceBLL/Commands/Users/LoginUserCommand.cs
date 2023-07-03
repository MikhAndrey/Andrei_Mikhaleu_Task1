using Microsoft.AspNetCore.Http;
using TripsServiceBLL.DTO.Users;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Utils;

namespace TripsServiceBLL.Commands.Users;

public class LoginUserCommand : ICommandAsync<UserLoginDTO>
{
	private IHttpContextAccessor _httpContextAccessor;

	private IUserService _userService;

	public LoginUserCommand(IHttpContextAccessor httpContextAccessor, IUserService userService)
	{
		_httpContextAccessor = httpContextAccessor;
		_userService = userService;
	}

	public async Task ExecuteAsync(UserLoginDTO dto)
	{
		string jwtToken = await _userService.GetJWTTokenAsync(dto);
		DateTime? cookieExpiresUTC = dto.RememberMe
			? DateTime.UtcNow.AddDays(UtilConstants.AuthorizationExpirationInDays)
			: null;
		_httpContextAccessor.HttpContext.Response.Cookies.Append(UtilConstants.JwtTokenCookiesAlias, jwtToken,
			new CookieOptions
			{
				HttpOnly = true,
				Secure = true,
				Expires = cookieExpiresUTC,
				SameSite = SameSiteMode.Strict
			});
	}
}