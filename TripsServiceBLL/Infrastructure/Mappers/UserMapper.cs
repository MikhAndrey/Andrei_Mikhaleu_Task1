using AutoMapper;
using TripsServiceBLL.DTO.Users;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Infrastructure.Mappers
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserSignupDTO, UserLoginDTO>();
        }
    }
}
