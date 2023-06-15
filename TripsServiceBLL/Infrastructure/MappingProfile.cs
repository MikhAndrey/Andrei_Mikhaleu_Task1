using AutoMapper;
using TripsServiceBLL.DTO.Comments;
using TripsServiceBLL.DTO.Images;
using TripsServiceBLL.DTO.RoutePoints;
using TripsServiceBLL.DTO.Trips;
using TripsServiceBLL.DTO.Users;
using TripsServiceBLL.Utils;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            _ = CreateMap<Comment, CommentDTO>();
            _ = CreateMap<Image, ImageDTO>();
            _ = CreateMap<RoutePoint, RoutePointDTO>();
            _ = CreateMap<Trip, ReadTripDTO>();
            _ = CreateMap<User, UserDTO>();
            _ = CreateMap<UserSignupDTO, UserLoginDTO>();
            _ = CreateMap<List<ReadTripDTO>, IQueryable<Trip>>();
            _ = CreateMap<CreateTripDTO, Trip>()
                .ForMember(trip => trip.StartTime, opt => opt.MapFrom
                (src => src.StartTime.AddSeconds(-src.StartTimeZoneOffset)))
                .ForMember(trip => trip.EndTime, opt => opt.MapFrom
                (src => src.EndTime.AddSeconds(-src.FinishTimeZoneOffset)));
            _ = CreateMap<EditTripDTO, Trip>()
                .IncludeBase<CreateTripDTO, Trip>()
                .ForMember(trip => trip.Id, opt => opt.Ignore())
                .ForMember(trip => trip.UserId, opt => opt.Ignore())
                .ForMember(trip => trip.Images, opt => opt.Ignore());
            _ = CreateMap<Trip, EditTripDTO>()
                .ForMember(dto => dto.StartTime, opt => opt.MapFrom(src => src.StartTime.AddSeconds(src.StartTimeZoneOffset)))
                .ForMember(dto => dto.EndTime, opt => opt.MapFrom(src => src.EndTime.AddSeconds(src.FinishTimeZoneOffset)));
            _ = CreateMap<Trip, ReadTripDTO>()
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime.AddSeconds(src.StartTimeZoneOffset)))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime.AddSeconds(src.FinishTimeZoneOffset)))
                .ForMember(dest => dest.IsCurrent, opt => opt.MapFrom(src => DateTime.UtcNow >= src.StartTime && DateTime.UtcNow <= src.EndTime))
                .ForMember(dest => dest.IsFuture, opt => opt.MapFrom(src => DateTime.UtcNow < src.StartTime))
                .ForMember(dest => dest.IsPast, opt => opt.MapFrom(src => DateTime.UtcNow > src.EndTime))
                .ForMember(dest => dest.UtcStartTimeZone, opt => opt.MapFrom
                (src => string.Concat(src.StartTime.AddSeconds(src.StartTimeZoneOffset).ToString("dd.MM.yyyy HH:mm"),
                $" UTC{src.StartTimeZoneOffset / 3600:+#;-#;+0}")))
                .ForMember(dest => dest.UtcFinishTimeZone, opt => opt.MapFrom
                (src => string.Concat(src.EndTime.AddSeconds(src.FinishTimeZoneOffset).ToString("dd.MM.yyyy HH:mm"),
                $" UTC{src.FinishTimeZoneOffset / 3600:+#;-#;+0}")));
            _ = CreateMap<Trip, ReadTripDTOExtended>()
                .IncludeBase<Trip, ReadTripDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));
            _ = CreateMap<Trip, TripDetailsDTO>()
                .IncludeBase<Trip, ReadTripDTO>()
                .ForMember(dest => dest.IsCurrentUserTrip, opt => opt.Ignore())
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src =>
                TimeUtils.GetTimeSpanString(src.EndTime.AddSeconds(src.StartTimeZoneOffset - src.FinishTimeZoneOffset) - src.StartTime)));
        }
    }
}
