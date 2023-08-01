﻿using TripsServiceBLL.DTO.Chats;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Interfaces;

public interface IChatService
{
    Task AddChatAsync(Chat chat);
    Task AddChatParticipationAsync(ChatParticipation chatParticipation);
    Task AddChatMessageAsync(ChatMessage chatMessage);
    IEnumerable<ChatListDTO> GetAll();
    Task<ChatDetailsDTO> GetById(int id);
    ChatMessage CreateMessageAboutChatJoining(int chatParticipationId);
    Task<int?> GetEmptyChatParticipationId(int chatId);
}
