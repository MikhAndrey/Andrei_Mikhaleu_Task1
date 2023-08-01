using AutoMapper;
using TripsServiceBLL.DTO.Chats;
using TripsServiceBLL.Interfaces;
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

    public async Task<ChatListDTO> AddAsync(ChatCreateDTO chat)
    {
        Chat chatToAdd = _mapper.Map<Chat>(chat);
        await _unitOfWork.Chats.AddAsync(chatToAdd);
        await _unitOfWork.SaveAsync();
        ChatListDTO chatToReturn = _mapper.Map<ChatListDTO>(chatToAdd);
        return chatToReturn;
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

    public async Task AddUser(int chatId)
    {
        _unitOfWork.Chats.ThrowErrorIfNotExists(chatId);
        int userId = _userService.GetCurrentUserId();
        ChatParticipation participation = new()
        {
            ChatId = chatId,
            UserId = userId
        };
        await _unitOfWork.ChatParticipations.AddAsync(participation);
        await _unitOfWork.SaveAsync();
    }
}
