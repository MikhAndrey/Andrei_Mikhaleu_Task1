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

    public ChatService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
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
}
