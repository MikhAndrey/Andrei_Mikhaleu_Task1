using Microsoft.AspNetCore.SignalR;
using TripsServiceBLL.Utils;

namespace TripsServiceBLL.Infrastructure.Common;

public class CustomUserIdProvider : IUserIdProvider
{
    public virtual string GetUserId(HubConnectionContext connection)
    {
        return connection.User.Claims.FirstOrDefault(c => c.Type == UtilConstants.UserIdClaimName)?.Value;
    }
}
