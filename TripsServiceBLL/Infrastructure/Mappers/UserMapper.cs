using AutoMapper;
using TripsServiceBLL.DTO.Users;
using TripsServiceBLL.Utils;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Infrastructure.Mappers
{
	public class UserMapper : Profile
	{
		public UserMapper()
		{
			CreateMap<User, UserDTO>();
			CreateMap<UserSignupDTO, UserLoginDTO>();
			CreateMap<UserSignupDTO, User>()
				.ForMember(user => user.Password, opt => opt.MapFrom(src => UtilEncryptor.Encrypt(src.Password)));
		}
	}
}
