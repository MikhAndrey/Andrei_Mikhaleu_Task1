using AutoMapper;
using System.Text.Json;
using TripsServiceBLL.DTO.Trips;
using TripsServiceBLL.Utils;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Infrastructure.Mappers
{
    public class TripMapper : Profile
    {
        public TripMapper()
        {
            CreateMap<Trip, ReadTripDTO>();
            CreateMap<List<ReadTripDTO>, IQueryable<Trip>>();
            CreateMap<Trip, EditPastTripDTO>();
            CreateMap<EditPastTripDTO, Trip>()
                .ForMember(trip => trip.Id, opt => opt.Ignore())
                .ForMember(trip => trip.UserId, opt => opt.Ignore())
                .ForMember(trip => trip.Images, opt => opt.Ignore());
            CreateMap<CreateTripDTO, Trip>()
                .ForMember(trip => trip.StartTime, opt => opt.MapFrom
                (src => src.StartTime.AddSeconds(-src.StartTimeZoneOffset)))
                .ForMember(trip => trip.EndTime, opt => opt.MapFrom
                (src => src.EndTime.AddSeconds(-src.FinishTimeZoneOffset)))
                .ForMember(trip => trip.RoutePoints, opt => opt.MapFrom(src => JsonSerializer.Deserialize<List<RoutePoint>>(src.RoutePointsAsString, null as JsonSerializerOptions)))
                .ForMember(trip => trip.Images, opt => opt.Ignore());
            CreateMap<EditTripDTO, Trip>()
                .IncludeBase<CreateTripDTO, Trip>()
                .ForMember(trip => trip.Id, opt => opt.Ignore())
                .ForMember(trip => trip.UserId, opt => opt.Ignore());
            CreateMap<Trip, EditTripDTO>()
                .ForMember(dto => dto.StartTime, opt => opt.MapFrom(src => src.StartTime.AddSeconds(src.StartTimeZoneOffset)))
                .ForMember(dto => dto.EndTime, opt => opt.MapFrom(src => src.EndTime.AddSeconds(src.FinishTimeZoneOffset)));
            CreateMap<Trip, ReadTripDTO>()
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
                $" UTC{src.FinishTimeZoneOffset / 3600:+#;-#;+0}")))
                .ForMember(dest => dest.IsNeedToBeRated, opt => opt.MapFrom((src, dest) => src.DriverId != null && src.Feedback == null && dest.IsPast))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom((src, dest) =>
                {
                    return src.DriverId != null && src.Feedback == null && dest.IsPast ? null : (src.Feedback?.Rating);
                }));
            CreateMap<Trip, ReadTripDTOExtended>()
                .IncludeBase<Trip, ReadTripDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));
            CreateMap<Trip, TripDetailsDTO>()
                .IncludeBase<Trip, ReadTripDTO>()
                .ForMember(dest => dest.IsCurrentUserTrip, opt => opt.Ignore())
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src =>
                TimeUtils.GetTimeSpanString(src.EndTime - src.StartTime)));
        }
    }
}
