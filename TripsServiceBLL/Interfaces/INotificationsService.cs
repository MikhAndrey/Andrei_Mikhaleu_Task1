using TripsServiceBLL.DTO.Chats;

namespace TripsServiceBLL.Interfaces;

public interface INotificationsService
{
    void StoreNotification(string userId, ChatNotificationMessageDTO notification);
    List<ChatNotificationMessageDTO> GetCurrentNotifications(string userId);
    void SetId(ChatNotificationMessageDTO notification);
    void Delete(string userId, int id);
}
