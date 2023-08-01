using AutoMapper;
using TripsServiceBLL.DTO.Chats;
using TripsServiceBLL.Interfaces;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Infrastructure.ValueResolvers;

public class CurrentUserChatResolver : IValueResolver<Chat, ChatDetailsDTO, bool>
{
    private readonly IUserService _userService;

    public CurrentUserChatResolver(IUserService userService)
    {
        _userService = userService;
    }

    public bool Resolve(Chat chat, ChatDetailsDTO dto, bool isCurrentUserInChat, ResolutionContext context)
    {
        int userId = _userService.GetCurrentUserId();
        return chat.ChatParticipations.Any(cp => cp.UserId == userId);
    }
}
