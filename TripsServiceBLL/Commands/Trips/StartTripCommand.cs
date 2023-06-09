﻿using TripsServiceDAL.Entities;
using TripsServiceBLL.Interfaces;

namespace TripsServiceBLL.Commands.Trips
{
	public class StartTripCommand : IAsyncCommand
	{
		private readonly ITripService _tripService;

		private readonly int _id;

		public StartTripCommand(ITripService tripService, int id)
		{
			_tripService = tripService;
			_id = id;
		}

		public async Task ExecuteAsync()
		{
			Trip? trip = await _tripService.GetByIdAsync(_id);
			if (trip != null)
			{
				if (trip.StartTime > DateTime.UtcNow)
				{
					_tripService.SetNewTimeForStartingTrip(trip);
					await _tripService.UpdateAsync(trip);
				}
			}
		}
	}
}
