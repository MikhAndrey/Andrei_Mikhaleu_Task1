using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using TripsServiceBLL.DTO.Chats;
using TripsServiceBLL.Infrastructure.Exceptions;
using TripsServiceBLL.Interfaces;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Interfaces;

namespace TripsServiceBLL.Commands.Chats;

public class ChatJoiningCommand : ICommandAsync<int, ChatJoinDTO>
{
	private readonly IChatService _chatService;
	private readonly IUserService _userService;

	private readonly IUnitOfWork _unitOfWork;

	private readonly IMapper _mapper;

	public ChatJoiningCommand(
		IChatService chatService,
		IUserService userService,
		IUnitOfWork unitOfWork,
		IMapper mapper)
	{
		_chatService = chatService;
		_userService = userService;
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

	public async Task<ChatJoinDTO> ExecuteAsync(int id)
	{
		_unitOfWork.Chats.ThrowErrorIfNotExists(id);
		
		int userId = _userService.GetCurrentUserId();
		ChatParticipation participation = new()
		{
			ChatId = id,
			UserId = userId
		};

		int? emptyChatParticipationId = await _chatService.GetEmptyChatParticipationId(id);

		using IDbContextTransaction transaction = _unitOfWork.BeginTransaction();
		try
		{
			ChatMessage? messageAboutChatJoining = null;
			await _chatService.AddChatParticipationAsync(participation);
			if (emptyChatParticipationId != null)
			{
				messageAboutChatJoining = _chatService.CreateMessageAboutChatJoining((int)emptyChatParticipationId);
				await _chatService.AddChatMessageAsync(messageAboutChatJoining);
			}
			await transaction.CommitAsync();
			ChatMessageDTO chatMessageDto = _mapper.Map<ChatMessageDTO>(messageAboutChatJoining);
			return new ChatJoinDTO
			{
				ChatParticipationId = participation.Id,
				Message = chatMessageDto
			};
		}
		catch (Exception)
		{
			await transaction.RollbackAsync();
			throw new DbOperationException();
		}
	}
}
