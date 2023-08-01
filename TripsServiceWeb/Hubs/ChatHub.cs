using Microsoft.AspNetCore.SignalR;
using TripsServiceBLL.DTO.Chats;

namespace Andrei_Mikhaleu_Task1.Hubs;

public class ChatHub : Hub
{
    public async Task BroadcastMessage(ChatMessageDTO message)
    {
        await Clients.All.SendAsync("BroadcastMessage", message);
    }
}
