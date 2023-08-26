using Microsoft.EntityFrameworkCore.Storage;
using TripsServiceBLL.Infrastructure.Exceptions;
using TripsServiceBLL.Interfaces;
using TripsServiceDAL.Interfaces;

namespace TripsServiceBLL.Commands.Chats;

public class ChatDeleteCommand : ICommandAsync<int>
{
    private readonly IChatService _chatService;

    private readonly IUnitOfWork _unitOfWork;

    public ChatDeleteCommand(IChatService chatService, IUnitOfWork unitOfWork)
    {
        _chatService = chatService;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int id)
    {
        using IDbContextTransaction transaction = _unitOfWork.BeginTransaction();
        try
        {
            await _chatService.DeleteChatMessagesWithoutSavingAsync(id);
            await _chatService.DeleteChatParticipationsWithoutSavingAsync(id);
            await _chatService.DeleteWithoutSavingAsync(id);
            await _unitOfWork.SaveAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw new DbOperationException();
        }
    }
}
