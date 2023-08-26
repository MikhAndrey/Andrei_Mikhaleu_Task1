using AutoMapper;
using TripsServiceBLL.DTO.Trips;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Utils;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Infrastructure.ValueResolvers;

public class CurrentUserTripResolver : IValueResolver<Trip, TripDetailsDTO, bool>
{
	private readonly IUserService _userService;

	public CurrentUserTripResolver(IUserService userService)
	{
		_userService = userService;
	}

	public bool Resolve(Trip trip, TripDetailsDTO dto, bool isCurrentUserTrip, ResolutionContext context)
	{
		int userId = _userService.GetCurrentUserId();
		string? currentUserRole = _userService.GetCurrentUserRole();
		return trip.UserId == userId || currentUserRole == UtilConstants.AdminRoleName;
	}
}
