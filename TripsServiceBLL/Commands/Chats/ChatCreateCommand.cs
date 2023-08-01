using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using TripsServiceBLL.DTO.Chats;
using TripsServiceBLL.Infrastructure.Exceptions;
using TripsServiceBLL.Interfaces;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Interfaces;

namespace TripsServiceBLL.Commands.Chats;

public class ChatCreateCommand : ICommandAsync<ChatCreateDTO, ChatListDTO>
{
	private readonly IUnitOfWork _unitOfWork;

	private readonly IMapper _mapper;

	private readonly IChatService _chatService;

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
		Chat chatToAdd = _mapper.Map<Chat>(chat);
		
		using IDbContextTransaction transaction = _unitOfWork.BeginTransaction();
		try
		{
			await _chatService.AddChatAsync(chatToAdd);
			ChatParticipation commonChatParticipation = new ChatParticipation
			{
				ChatId = chatToAdd.Id,
				UserId = null
			};
			await _chatService.AddChatParticipationAsync(commonChatParticipation);
			await transaction.CommitAsync();
		} catch (Exception)
		{
			await transaction.RollbackAsync();
			throw new DbOperationException();
		}
		
		ChatListDTO chatToReturn = _mapper.Map<ChatListDTO>(chatToAdd);
		return chatToReturn;
	}
}
