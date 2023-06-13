using Microsoft.EntityFrameworkCore;
using TripsServiceBLL.DTO.Statistics;
using TripsServiceBLL.DTO.Trips;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Utils;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Interfaces;

namespace TripsServiceBLL.Services
{
	public class TripService : ITripService
	{
		private readonly IUnitOfWork _unitOfWork;

		public TripService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Trip?> GetByIdAsync(int id)
		{
			return await _unitOfWork.Trips.GetByIdAsync(id);
		}

		public void SetNewTimeForStartingTrip(Trip trip)
		{
			trip.EndTime -= trip.StartTime - DateTime.UtcNow;
			trip.EndTime = DateTime.Parse(trip.EndTime.ToString("dd.MM.yyyy HH:mm"));
			trip.StartTime = DateTime.UtcNow;
			trip.StartTime = DateTime.Parse(trip.StartTime.ToString("dd.MM.yyyy HH:mm"));
		}

		public void SetNewTimeForEndingTrip(Trip trip)
		{
			trip.EndTime = DateTime.UtcNow;
			trip.EndTime = DateTime.Parse(trip.EndTime.ToString("dd.MM.yyyy HH:mm"));
		}

		public void FixTimeOfNewTripForTimeZones(CreateTripDTO trip)
		{
			trip.StartTime = trip.StartTime.AddSeconds(-trip.StartTimeZoneOffset);
			trip.EndTime = trip.EndTime.AddSeconds(-trip.FinishTimeZoneOffset);
		}

		public void UpdateFromEditTripDTO(Trip trip, EditTripDTO editTrip)
		{
			trip.Name = editTrip.Name;
			trip.Description = editTrip.Description;
			trip.Distance = editTrip.Distance;
			trip.Public = editTrip.Public;
			trip.StartTime = editTrip.StartTime.AddSeconds(-editTrip.StartTimeZoneOffset);
			trip.EndTime = editTrip.EndTime.AddSeconds(-editTrip.FinishTimeZoneOffset); ;
			trip.StartTimeZoneOffset = editTrip.StartTimeZoneOffset;
			trip.FinishTimeZoneOffset = editTrip.FinishTimeZoneOffset;
		}

		public async Task UpdateAsync(Trip trip)
		{
			_unitOfWork.Trips.Update(trip);
			await _unitOfWork.SaveAsync();
		}

		public async Task DeleteAsync(Trip trip)
		{
			_unitOfWork.Trips.Delete(trip);
			await _unitOfWork.SaveAsync();
		}

		public async Task AddAsync(Trip trip)
		{
			await _unitOfWork.Trips.AddAsync(trip);
			await _unitOfWork.SaveAsync();
		}

		public async Task<TripDetailsDTO> GetTripDetailsAsync(int tripId, int userId)
		{
			Trip? trip = await GetByIdAsync(tripId);
			return new(trip, userId);
		}

		public async Task<EditTripDTO> GetTripForEditingAsync(int tripId)
		{
			Trip? trip = await GetByIdAsync(tripId);
			return new(trip);
		}

		public IQueryable<ReadTripDTOExtended> GetOthersPublicTrips(int userId)
		{
			IQueryable<Trip> rawTrips = _unitOfWork.Trips.GetOthersPublicTrips(userId);
			return rawTrips.Select(el => new ReadTripDTOExtended(el));
		}

		public IQueryable<ReadTripDTO> GetHistoryOfTripsByUserId(int userId)
		{
			IQueryable<Trip> rawTrips = _unitOfWork.Trips.GetHistoryOfTripsByUserId(userId);
			return rawTrips.Select(el => new ReadTripDTO(el));
		}

		public IQueryable<ReadTripDTO> GetTripsByUserId(int userId)
		{
			IQueryable<Trip> rawTrips = _unitOfWork.Trips.GetTripsByUserId(userId);
			return rawTrips.Select(el => new ReadTripDTO(el));
		}

		public YearsStatisticsDTO GetYearsOfUserTrips(int userId)
		{
			IQueryable<int> years = _unitOfWork.Trips.GetYearsOfUserTrips(userId);
			return new()
			{
				Years = years,
				SelectedYear = years.FirstOrDefault()
			};
		}

		public async Task<List<DurationInMonth>> GetTotalDurationByMonthsAsync(int year, int userId)
		{
			List<Trip> trips = await _unitOfWork.Trips.GetTripsByYearAndUserId(year, userId).ToListAsync();

			if (trips.Count == 0)
			{
				return new();
			}

			List<DurationInMonth> result = Enumerable.Range(1, 12)
				.Select(month => new DurationInMonth()
				{
					Month = new DateTime(year, month, 1).ToString("MMMM"),
					TotalDuration = trips
						.Where(t => t.StartTime.Year == year && t.StartTime.Month <= month && t.EndTime.Year == year && t.EndTime.Month >= month)
						.Select(t =>
						{
							DateTime start = t.StartTime <= new DateTime(year, month, 1) ? new DateTime(year, month, 1) : t.StartTime;
							DateTime end = t.EndTime >= new DateTime(year, month, DateTime.DaysInMonth(year, month)) ?
							new DateTime(year, month, DateTime.DaysInMonth(year, month)) : t.EndTime;
							return (end - start).TotalHours;
						})
					.DefaultIfEmpty(0)
					.Sum()
				}).ToList();
			return result;
		}
	}
}
