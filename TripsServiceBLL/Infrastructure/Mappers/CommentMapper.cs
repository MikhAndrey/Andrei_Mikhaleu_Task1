using AutoMapper;
using TripsServiceBLL.DTO.Comments;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Infrastructure.Mappers
{
	public class CommentMapper : Profile
	{
		public CommentMapper()
		{
			CreateMap<Comment, CommentDTO>();
		}
	}
}
