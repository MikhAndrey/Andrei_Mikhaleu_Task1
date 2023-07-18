using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TripsServiceBLL.DTO.Trips;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Utils;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Interfaces;

namespace TripsServiceBLL.Services;

public class TripService : ITripService
{
	private readonly IMapper _mapper;

	private readonly IUnitOfWork _unitOfWork;

	private readonly IUserService _userService;

	public TripService(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_userService = userService;
	}

	public void SetNewTimeForStartingTrip(Trip trip)
	{
		trip.EndTime -= trip.StartTime - DateTime.UtcNow;
		trip.EndTime = DateTime.Parse(trip.EndTime.ToString("dd.MM.yyyy HH:mm"));
		trip.StartTime = DateTime.UtcNow;
		trip.StartTime = DateTime.Parse(trip.StartTime.ToString("dd.MM.yyyy HH:mm"));
	}

	public async Task<TripDateChangesDTO> StartTripAsync(int tripId)
	{
		Trip trip = await _unitOfWork.Trips.GetByIdAsync(tripId);

		if (trip.StartTime > DateTime.UtcNow)
		{
			SetNewTimeForStartingTrip(trip);
			await UpdateAsync(trip);
		}

		TripDateChangesDTO result = _mapper.Map<TripDateChangesDTO>(trip);
		return result;
	}

	public void SetNewTimeForEndingTrip(Trip trip)
	{
		trip.EndTime = DateTime.UtcNow;
		trip.EndTime = DateTime.Parse(trip.EndTime.ToString("dd.MM.yyyy HH:mm"));
	}

	public async Task<TripDateChangesDTO> EndTripAsync(int tripId)
	{
		Trip trip = await _unitOfWork.Trips.GetByIdAsync(tripId);

		if (trip.StartTime < DateTime.UtcNow && trip.EndTime > DateTime.UtcNow)
		{
			SetNewTimeForEndingTrip(trip);
			await UpdateAsync(trip);
		}
		
		TripDateChangesDTO result = _mapper.Map<TripDateChangesDTO>(trip);
		return result;
	}

	public async Task UpdateAsync(Trip trip)
	{
		_unitOfWork.Trips.Update(trip);
		await _unitOfWork.SaveAsync();
	}

	public async Task DeleteAsync(int id)
	{
		Trip trip = await _unitOfWork.Trips.GetByIdAsync(id);
		_unitOfWork.Trips.Delete(trip);
		await _unitOfWork.SaveAsync();
	}

	public async Task AddAsync(Trip trip)
	{
		await _unitOfWork.Trips.AddAsync(trip);
		await _unitOfWork.SaveAsync();
	}

	public async Task<TripDetailsDTO> GetTripDetailsAsync(int tripId)
	{
		Trip trip = await _unitOfWork.Trips.GetByIdForDetailsAsync(tripId);
		TripDetailsDTO dto = _mapper.Map<TripDetailsDTO>(trip);
		return dto;
	}

	public async Task<EditTripDTO> GetTripForEditingAsync(int tripId)
	{
		Trip trip = await _unitOfWork.Trips.GetByIdForEditingAsync(tripId);
		EditTripDTO dto = _mapper.Map<EditTripDTO>(trip);
		return dto;
	}

	public async Task<EditPastTripDTO> GetPastTripForEditingAsync(int tripId)
	{
		Trip trip = await _unitOfWork.Trips.GetByIdForMinimalEditingAsync(tripId);
		EditPastTripDTO dto = _mapper.Map<EditPastTripDTO>(trip);
		return dto;
	}

	public IEnumerable<ReadTripDTOExtended> GetOthersPublicTrips()
	{
		int userId = _userService.GetCurrentUserId();
		IEnumerable<Trip> rawTrips = _unitOfWork.Trips.GetOthersPublicTrips(userId);
		IEnumerable<ReadTripDTOExtended> mappedTrips = rawTrips.Select(el =>
		{
			ReadTripDTOExtended mappedTrip = _mapper.Map<ReadTripDTOExtended>(el);
			return mappedTrip;
		});
		return mappedTrips;
	}

	public IEnumerable<ReadTripDTO> GetCurrentUserHistoryOfTrips()
	{
		int userId = _userService.GetCurrentUserId();
		IEnumerable<Trip> rawTrips = _unitOfWork.Trips.GetHistoryOfTripsByUserId(userId);
		IEnumerable<ReadTripDTO> mappedTrips = rawTrips.Select(el =>
		{
			ReadTripDTO mappedTrip = _mapper.Map<ReadTripDTO>(el);
			return mappedTrip;
		});
		return mappedTrips;
	}

	public IEnumerable<ReadTripDTO> GetCurrentUserTrips()
	{
		int userId = _userService.GetCurrentUserId();
		IEnumerable<Trip> rawTrips = _unitOfWork.Trips.GetTripsByUserId(userId);
		IEnumerable<ReadTripDTO> mappedTrips = rawTrips.Select(el =>
		{
			ReadTripDTO mappedTrip = _mapper.Map<ReadTripDTO>(el);
			return mappedTrip;
		});
		return mappedTrips;
	}

	public IEnumerable<int> GetYearsOfCurrentUserTrips()
	{
		int userId = _userService.GetCurrentUserId();
		IEnumerable<int> years = _unitOfWork.Trips.GetYearsOfUserTrips(userId);
		return years;
	}

	public async Task<List<UtilDurationInMonth>> GetTotalDurationByMonthsAsync(int year)
	{
		int userId = _userService.GetCurrentUserId();
		List<Trip> trips = await _unitOfWork.Trips.GetTripsByYearAndUserId(year, userId).ToListAsync();

		if (trips.Count == 0)
		{
			return new List<UtilDurationInMonth>();
		}

		List<UtilDurationInMonth> result = Enumerable.Range(1, 12)
			.Select(month => new UtilDurationInMonth
			{
				Month = new DateTime(year, month, 1).ToString("MMMM"),
				TotalDuration = trips
					.Where(t => t.StartTime.Year == year && t.StartTime.Month <= month && t.EndTime.Year == year &&
								t.EndTime.Month >= month)
					.Select(t =>
					{
						DateTime start = t.StartTime <= new DateTime(year, month, 1)
							? new DateTime(year, month, 1)
							: t.StartTime;
						DateTime end = t.EndTime >= new DateTime(year, month, DateTime.DaysInMonth(year, month))
							? new DateTime(year, month, DateTime.DaysInMonth(year, month))
							: t.EndTime;
						return (end - start).TotalHours;
					})
					.DefaultIfEmpty(0)
					.Sum()
			}).ToList();
		return result;
	}
}
