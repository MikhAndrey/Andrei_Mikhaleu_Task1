using AutoMapper;
using TripsServiceBLL.DTO.Chats;
using TripsServiceBLL.Infrastructure.Exceptions;
using TripsServiceBLL.Interfaces;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Interfaces;

namespace TripsServiceBLL.Commands.Chats;

public class ChatCreateCommand : ICommandAsync<ChatCreateDTO, ChatListDTO>
{
    private readonly IChatService _chatService;

    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public ChatCreateCommand(
        IChatService chatService,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _chatService = chatService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ChatListDTO> ExecuteAsync(ChatCreateDTO chat)
    {
        var chatToAdd = _mapper.Map<Chat>(chat);

        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            await _chatService.AddChatAsync(chatToAdd);
            var commonChatParticipation = new ChatParticipation
            {
                ChatId = chatToAdd.Id,
                UserId = null
            };
            await _chatService.AddChatParticipationAsync(commonChatParticipation);
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw new DbOperationException();
        }

        var chatToReturn = _mapper.Map<ChatListDTO>(chatToAdd);
        return chatToReturn;
    }
}
