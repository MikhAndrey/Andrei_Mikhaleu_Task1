using AutoMapper;
using TripsServiceBLL.DTO.Feedbacks;
using TripsServiceBLL.Infrastructure.ValueResolvers;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Infrastructure.Mappers
{
    public class FeedbackMapper : Profile
    {
        public FeedbackMapper(FeedbackUserResolver feedbackUserResolver)
        {
            CreateMap<Trip, ReadFeedbackDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Feedback.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Feedback.Text))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Feedback.Rating))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));
            CreateMap<CreateFeedbackDTO, Feedback>();
            CreateMap<CreateFeedbackDTO, ReadFeedbackDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(feedbackUserResolver));
        }
    }
}
