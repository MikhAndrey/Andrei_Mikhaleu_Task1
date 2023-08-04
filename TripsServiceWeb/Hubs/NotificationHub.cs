using Microsoft.AspNetCore.SignalR;
using TripsServiceBLL.DTO.Chats;
using TripsServiceBLL.Interfaces;

namespace Andrei_Mikhaleu_Task1.Hubs;

public class NotificationHub : Hub
{
   private INotificationsService _notificationsService;

   public NotificationHub(INotificationsService notificationsService)
   {
      _notificationsService = notificationsService;
   }
   
   public async Task Notify(List<string> receivers, ChatNotificationMessageDTO message)
   {
      await Clients.Users(receivers).SendAsync("BroadcastChatNotification", message);
      foreach (string userId in receivers)
      {
         _notificationsService.StoreNotification(userId, message);
      }
   }
}
