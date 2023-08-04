using Microsoft.Extensions.Caching.Memory;
using TripsServiceBLL.DTO.Chats;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Utils;

namespace TripsServiceBLL.Services;

public class NotificationsService : INotificationsService
{
    private readonly IMemoryCache _cache;

    public NotificationsService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public void StoreNotification(string userId, ChatNotificationMessageDTO notification)
    {
        List<ChatNotificationMessageDTO> notifications = GetCurrentNotifications(userId);
        notifications.Add(notification);
        _cache.Set(UtilConstants.NotificationsCacheStorageNamePrefix + userId, notifications);
    }

    public List<ChatNotificationMessageDTO> GetCurrentNotifications(string userId)
    {
        if (_cache.TryGetValue<List<ChatNotificationMessageDTO>>(UtilConstants.NotificationsCacheStorageNamePrefix + userId, out var notifications))
        {
            return notifications ?? new List<ChatNotificationMessageDTO>();
        }
        
        return new List<ChatNotificationMessageDTO>();
    }
}
