using AutoMapper;
using TripsServiceBLL.DTO.Comments;
using TripsServiceBLL.Infrastructure.ValueResolvers;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Infrastructure.Mappers;

public class CommentMapper : Profile
{
	public CommentMapper(CommentUserIdResolver commentUserIdResolver)
	{
		CreateMap<Comment, CommentDTO>();
		CreateMap<CreateCommentDTO, Comment>()
			.ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateTime.UtcNow))
			.ForMember(dest => dest.UserId, opt => opt.MapFrom(commentUserIdResolver));
	}
}
