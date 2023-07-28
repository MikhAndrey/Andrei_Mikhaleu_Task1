using AutoMapper;
using TripsServiceBLL.DTO.Users;
using TripsServiceBLL.Infrastructure.ValueResolvers;
using TripsServiceBLL.Utils;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Infrastructure.Mappers;

public class UserMapper : Profile
{
	public UserMapper(NewUserRoleIdResolver userRoleIdResolver)
	{
		CreateMap<User, UserDTO>();
		CreateMap<UserSignupDTO, UserLoginDTO>();
		CreateMap<UserSignupDTO, User>()
			.ForMember(user => user.Password, opt => opt.MapFrom(src => UtilEncryptor.Encrypt(src.Password)))
			.ForMember(user => user.RoleId, opt => opt.MapFrom(userRoleIdResolver));
		CreateMap<User, UserListDTO>()
			.ForMember(user => user.Role, opt => opt.MapFrom(src => src.Role.Name));
	}
}
