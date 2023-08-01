﻿using AutoMapper;
using TripsServiceBLL.DTO.Chats;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Utils;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Interfaces;

namespace TripsServiceBLL.Services;

public class ChatService : IChatService
{
    private readonly IMapper _mapper;

    private readonly IUnitOfWork _unitOfWork;

    private readonly IUserService _userService;

    public ChatService(IMapper mapper, IUnitOfWork unitOfWork, IUserService userService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _userService = userService;
    }

    public async Task AddChatAsync(Chat chat)
    {
        await _unitOfWork.Chats.AddAsync(chat);
        await _unitOfWork.SaveAsync();
    }
    
    public async Task AddChatParticipationAsync(ChatParticipation chatParticipation)
    {
        await _unitOfWork.ChatParticipations.AddAsync(chatParticipation);
        await _unitOfWork.SaveAsync();
    }
    
    public async Task AddChatMessageAsync(ChatMessage chatMessage)
    {
        await _unitOfWork.ChatMessages.AddAsync(chatMessage);
        await _unitOfWork.SaveAsync();
    }
    
    public IEnumerable<ChatListDTO> GetAll()
    {
        IEnumerable <Chat> rawChats = _unitOfWork.Chats.GetAll();
        IEnumerable<ChatListDTO> mappedChats = rawChats.Select(el =>
        {
            ChatListDTO mappedChat = _mapper.Map<ChatListDTO>(el);
            return mappedChat;
        });
        return mappedChats;
    }

    public async Task<ChatDetailsDTO> GetById(int id)
    {
        Chat? chat = await _unitOfWork.Chats.GetByIdAsync(id);
        ChatDetailsDTO chatDetails = _mapper.Map<ChatDetailsDTO>(chat);
        return chatDetails;
    }

    public async Task<int?> GetEmptyChatParticipationId(int chatId)
    {
        ChatParticipation? chatParticipation = await _unitOfWork.ChatParticipations.GetEmptyChatParticipation(chatId);
        return chatParticipation?.Id;
    }

    public ChatMessage CreateMessageAboutChatJoining(int chatParticipationId)
    {
        string? userName = _userService.GetCurrentUserName();
        ChatMessage messageAboutChatJoining = new()
        {
            ChatParticipationId = chatParticipationId,
            Text = UtilConstants.ChatJoiningMessage(userName)
        };
        return messageAboutChatJoining;
    }
}
