using System.Security.Claims;
using TripsServiceBLL.Utils;

namespace Andrei_Mikhaleu_Task1.Helpers
{
	public static class UserHelper
	{
		public static int GetUserIdFromClaims(IEnumerable<Claim> claims)
		{
			return int.Parse(claims.FirstOrDefault(c => c.Type == Constants.UserIdClaimName)?.Value);
		}
	}
}
