using AutoMapper;
using TripsServiceBLL.DTO.Comments;
using TripsServiceBLL.Infrastructure.ValueResolvers;
using TripsServiceBLL.Utils;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Infrastructure.Mappers;

public class CommentMapper : Profile
{
	public CommentMapper(CommentUserIdResolver commentUserIdResolver)
	{
		CreateMap<Comment, CommentDTO>()
			.ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
			.ForMember(dest => dest.TimeAgoAsString, opt => opt.MapFrom(src => UtilDateTimeFunctions.GetTimeAgoFromNow(src.Date)));
		CreateMap<CreateCommentDTO, Comment>()
			.ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateTime.UtcNow))
			.ForMember(dest => dest.UserId, opt => opt.MapFrom(commentUserIdResolver));
	}
}
