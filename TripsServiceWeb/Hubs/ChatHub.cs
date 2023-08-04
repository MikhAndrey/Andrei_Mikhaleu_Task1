﻿using Microsoft.AspNetCore.SignalR;
using TripsServiceBLL.DTO.Chats;

namespace Andrei_Mikhaleu_Task1.Hubs;

public class ChatHub : Hub
{
    public async Task BroadcastMessage(ChatMessageDTO message, int chatId)
    {
        await Clients.Group(chatId.ToString()).SendAsync("BroadcastMessage", message);
    }

    public async Task AddUserToGroup(int chatId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
    }

    public async Task RemoveUserFromGroup(int chatId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
    }
}