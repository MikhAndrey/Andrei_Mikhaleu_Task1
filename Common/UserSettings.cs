using Andrei_Mikhaleu_Task1.Models.Entities.Common;
using Andrei_Mikhaleu_Task1.Models.Repos;

namespace Andrei_Mikhaleu_Task1.Common
{
    public class UserSettings
	{
		private readonly UserRepository _userRepository;

		public User CurrentUser { get; private set; }

		public UserSettings(UserRepository userRepository, IHttpContextAccessor httpContextAccessor)
		{
			_userRepository = userRepository;

			var username = httpContextAccessor.HttpContext?.User?.Identity?.Name;
			if (!string.IsNullOrEmpty(username))
			{
				CurrentUser = _userRepository.GetByUsername(username);
			}
		}
	}
}
