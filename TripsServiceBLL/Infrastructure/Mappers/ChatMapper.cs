using AutoMapper;
using TripsServiceBLL.DTO.Chats;
using TripsServiceBLL.DTO.Users;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Infrastructure.Mappers;

public class ChatMapper : Profile
{
    public ChatMapper()
    {
        CreateMap<ChatCreateDTO, Chat>();
        CreateMap<Chat, ChatListDTO>();
        CreateMap<ChatMessage, ChatMessageDTO>();
        CreateMap<ChatParticipation, UserChatDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.User.Role.Name))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));
        CreateMap<Chat, ChatDetailsDTO>()
            .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.ChatParticipations));
    }
}
