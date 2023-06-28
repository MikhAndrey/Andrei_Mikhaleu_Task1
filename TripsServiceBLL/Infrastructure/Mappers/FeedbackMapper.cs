using AutoMapper;
using TripsServiceBLL.DTO.Feedbacks;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Infrastructure.Mappers
{
	public class FeedbackMapper : Profile
	{
		public FeedbackMapper()
		{
			CreateMap<Trip, ReadFeedbackDTO>()
				.ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Feedback.Text))
				.ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Feedback.Rating))
				.ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));
			CreateMap<CreateFeedbackDTO, Feedback>();
		}
	}
}
