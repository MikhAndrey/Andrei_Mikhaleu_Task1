using TripsServiceBLL.DTO.Chats;

namespace TripsServiceBLL.Interfaces;

public interface INotificationsService
{
    void StoreNotification(string userId, ChatNotificationMessageDTO notification);

    List<ChatNotificationMessageDTO> GetCurrentNotifications(string userId);
}
