using AutoMapper;
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

    public async Task DeleteWithoutSavingAsync(int id)
    {
        Chat chat = await _unitOfWork.Chats.GetByIdAsync(id);
        _unitOfWork.Chats.Delete(chat);
    }

    public async Task DeleteChatMessagesWithoutSavingAsync(int id)
    {
        IQueryable<ChatMessage> messagesToDelete = _unitOfWork.ChatMessages.GetByChatId(id);
        foreach (ChatMessage message in messagesToDelete)
        {
            _unitOfWork.ChatMessages.Delete(message);
        }
    }
    
    public async Task DeleteChatParticipationsWithoutSavingAsync(int id)
    {
        IQueryable<ChatParticipation> participationsToDelete = _unitOfWork.ChatParticipations.GetByChatId(id);
        foreach (ChatParticipation participation in participationsToDelete)
        {
            _unitOfWork.ChatParticipations.Delete(participation);
        }
    }
    
    public async Task AddChatParticipationAsync(ChatParticipation chatParticipation)
    {
        await _unitOfWork.ChatParticipations.AddAsync(chatParticipation);
        await _unitOfWork.SaveAsync();
    }

    public async Task<ChatMessageDTO> LeaveChatAsync(ChatLeaveDTO dto)
    {
        ChatParticipation chatParticipationToModify = await _unitOfWork.ChatParticipations.GetByIdAsync(dto.ParticipationId);
        await DeactivateChatParticipationAsync(chatParticipationToModify);
        
        int? emptyChatParticipationId = await GetEmptyChatParticipationIdAsync(dto.ChatId);
        ChatMessage messageAboutChatLeaving = CreateMessageAboutChatLeaving((int)emptyChatParticipationId);
		await AddChatMessageAsync(messageAboutChatLeaving);
        
        ChatMessageDTO mappedLeavingMessage = _mapper.Map<ChatMessageDTO>(messageAboutChatLeaving);
        return mappedLeavingMessage;
    }

    public async Task ActivateChatParticipationAsync(ChatParticipation chatParticipation)
    {
        chatParticipation.IsActive = true;
        await _unitOfWork.SaveAsync();
    }
    
    public async Task DeactivateChatParticipationAsync(ChatParticipation chatParticipation)
    {
        chatParticipation.IsActive = false;
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

    public async Task<ChatDetailsDTO> GetByIdAsync(int id)
    {
        Chat? chat = await _unitOfWork.Chats.GetByIdAsync(id);
        ChatDetailsDTO chatDetails = _mapper.Map<ChatDetailsDTO>(chat);
        return chatDetails;
    }

    public async Task<int?> GetEmptyChatParticipationIdAsync(int chatId)
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
    
    public ChatMessage CreateMessageAboutChatLeaving(int chatParticipationId)
    {
        string? userName = _userService.GetCurrentUserName();
        ChatMessage messageAboutChatLeaving = new()
        {
            ChatParticipationId = chatParticipationId,
            Text = UtilConstants.ChatLeavingMessage(userName)
        };
        return messageAboutChatLeaving;
    }

    public async Task<ChatMessageDTO> SendMessageAsync(ChatSendMessageDTO dto)
    {
        ChatMessage messageToSend = _mapper.Map<ChatMessage>(dto);
        await AddChatMessageAsync(messageToSend);
        
        ChatMessageDTO result = _mapper.Map<ChatMessageDTO>(messageToSend);
        result.User = _userService.GetCurrentUserMainInfo();
        return result;
    }

    public async Task<int> GetCurrentChatParticipationIdAsync(int chatId)
    {
        int userId = _userService.GetCurrentUserId();
        ChatParticipation? chatParticipation = await _unitOfWork.ChatParticipations.GetByChatIdAndUserId(chatId, userId);
        if (chatParticipation == null)
            _unitOfWork.ChatParticipations.ThrowErrorIfEntityIsNull(chatParticipation);
        return chatParticipation.Id;
    }
}
