using AutoMapper;
using TripsServiceBLL.DTO.Chats;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Infrastructure.Mappers;

public class ChatMapper : Profile
{
    public ChatMapper()
    {
        CreateMap<ChatCreateDTO, Chat>();
        CreateMap<Chat, ChatListDTO>();
    }
}
