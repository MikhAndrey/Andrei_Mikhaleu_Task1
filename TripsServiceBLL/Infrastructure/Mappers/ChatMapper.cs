using AutoMapper;
using TripsServiceBLL.DTO.Chats;
using TripsServiceBLL.DTO.Users;
using TripsServiceBLL.Infrastructure.ValueResolvers;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Infrastructure.Mappers;

public class ChatMapper : Profile
{
    public ChatMapper(CurrentUserChatResolver currentUserChatResolver)
    {
        CreateMap<ChatCreateDTO, Chat>();
        CreateMap<Chat, ChatListDTO>();
        CreateMap<ChatMessage, ChatMessageDTO>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.ChatParticipation.User));
        CreateMap<ChatSendMessageDTO, ChatMessage>();
        CreateMap<ChatParticipation, UserListDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.User.Role.Name))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));
        CreateMap<Chat, ChatDetailsDTO>()
            .ForMember(dest => dest.Messages, opt => 
                opt.MapFrom(src => src.ChatParticipations.SelectMany(cp => cp.ChatMessages).OrderBy(m => m.Id)))
            .ForMember(dest => dest.IsCurrentUserInChat, opt => opt.MapFrom(currentUserChatResolver));
    }
}
