using Microsoft.Extensions.Caching.Memory;
using TripsServiceBLL.DTO.Chats;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Utils;

namespace TripsServiceBLL.Services;

public class NotificationsService : INotificationsService
{
    private static int _currentId;  
    
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

    public void SetId(ChatNotificationMessageDTO notification)
    {
        _currentId++;
        notification.Id = _currentId;
    }

    public void Delete(string userId, int id)
    {
        if (_cache.TryGetValue<List<ChatNotificationMessageDTO>>(UtilConstants.NotificationsCacheStorageNamePrefix + userId, out var notifications))
        {
            if (notifications != null)
            {
                ChatNotificationMessageDTO? notificationToDelete =
                    notifications.Find(notification => notification.Id == id);
                if (notificationToDelete != null)
                {
                    notifications.Remove(notificationToDelete);
                    _cache.Set(UtilConstants.NotificationsCacheStorageNamePrefix + userId, notifications);
                }
            }
        }
    }
}
