using System.Security.Claims;
using TripsServiceBLL.Utils;

namespace TripsServiceBLL.Helpers
{
	public static class UserHelper
	{
		public static int GetUserIdFromClaims(IEnumerable<Claim> claims)
		{
			return int.Parse(claims.FirstOrDefault(c => c.Type == UtilConstants.UserIdClaimName)?.Value);
		}
	}
}
