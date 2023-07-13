using AutoMapper;
using TripsServiceBLL.DTO.Trips;
using TripsServiceBLL.Interfaces;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Infrastructure.ValueResolvers;

public class NewTripUserIdResolver : IValueResolver<CreateTripDTO, Trip, int>
{
	private readonly IUserService _userService;

	public NewTripUserIdResolver(IUserService userService)
	{
		_userService = userService;
	}

	public int Resolve(CreateTripDTO dto, Trip trip, int newTripUserId, ResolutionContext context)
	{
		return _userService.GetCurrentUserId();
	}
}
