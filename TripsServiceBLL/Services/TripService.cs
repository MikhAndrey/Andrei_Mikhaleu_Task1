using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TripsServiceBLL.DTO.Statistics;
using TripsServiceBLL.DTO.Trips;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Utils;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Infrastructure.Exceptions;
using TripsServiceDAL.Interfaces;

namespace TripsServiceBLL.Services
{
	public class TripService : ITripService
	{
		private readonly IUnitOfWork _unitOfWork;

		private readonly IMapper _mapper;

		public TripService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
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

		public async Task StartTripAsync(int tripId)
		{
			Trip? trip = await _unitOfWork.Trips.GetByIdAsync(tripId);
			if (trip == null)
			{
				throw new EntityNotFoundException(TripsServiceDAL.Utils.UtilConstants.GetEntityNotExistsMessage<Trip>()());
			}

			if (trip.StartTime > DateTime.UtcNow)
			{
				SetNewTimeForStartingTrip(trip);
				await UpdateAsync(trip);
			}
		}

		public void SetNewTimeForEndingTrip(Trip trip)
		{
			trip.EndTime = DateTime.UtcNow;
			trip.EndTime = DateTime.Parse(trip.EndTime.ToString("dd.MM.yyyy HH:mm"));
		}

		public async Task EndTripAsync(int tripId)
		{
			Trip? trip = await _unitOfWork.Trips.GetByIdAsync(tripId);
			if (trip == null)
			{
				throw new EntityNotFoundException(TripsServiceDAL.Utils.UtilConstants.GetEntityNotExistsMessage<Trip>()());
			}

			if (trip.StartTime < DateTime.UtcNow && trip.EndTime > DateTime.UtcNow)
			{
				SetNewTimeForEndingTrip(trip);
				await UpdateAsync(trip);
			}
		}

		public async Task UpdateAsync(Trip trip)
		{
			_unitOfWork.Trips.Update(trip);
			await _unitOfWork.SaveAsync();
		}

		public async Task DeleteAsync(int id)
		{
			Trip? trip = await _unitOfWork.Trips.GetByIdAsync(id);
			if (trip == null)
			{
				throw new EntityNotFoundException(TripsServiceDAL.Utils.UtilConstants.GetEntityNotExistsMessage<Trip>()());
			}

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
			Trip? trip = await _unitOfWork.Trips.GetByIdForDetailsAsync(tripId);
			if (trip == null)
			{
				throw new EntityNotFoundException(TripsServiceDAL.Utils.UtilConstants.GetEntityNotExistsMessage<Trip>()());
			}

			_unitOfWork.Users.ThrowErrorIfNotExists(userId);

			TripDetailsDTO dto = _mapper.Map<Trip, TripDetailsDTO>(trip, opt =>
				opt.AfterMap((src, dest) => dest.IsCurrentUserTrip = src.User.Id == userId));
			return dto;
		}

		public async Task<EditTripDTO> GetTripForEditingAsync(int tripId)
		{
			Trip? trip = await _unitOfWork.Trips.GetByIdForEditingAsync(tripId);
			if (trip == null)
			{
				throw new EntityNotFoundException(TripsServiceDAL.Utils.UtilConstants.GetEntityNotExistsMessage<Trip>()());
			}

			EditTripDTO dto = _mapper.Map<EditTripDTO>(trip);
			return dto;
		}

		public async Task<EditPastTripDTO> GetPastTripForEditingAsync(int tripId)
		{
			Trip? trip = await _unitOfWork.Trips.GetByIdForMinimalEditingAsync(tripId);
			if (trip == null)
			{
				throw new EntityNotFoundException(TripsServiceDAL.Utils.UtilConstants.GetEntityNotExistsMessage<Trip>()());
			}

			EditPastTripDTO dto = _mapper.Map<EditPastTripDTO>(trip);
			return dto;
		}

		public IQueryable<ReadTripDTOExtended> GetOthersPublicTrips(int userId)
		{
			_unitOfWork.Users.ThrowErrorIfNotExists(userId);

			IQueryable<Trip> rawTrips = _unitOfWork.Trips.GetOthersPublicTrips(userId);
			IQueryable<ReadTripDTOExtended> mappedTrips = rawTrips.Select(el => _mapper.Map<ReadTripDTOExtended>(el));
			return mappedTrips;
		}

		public IQueryable<ReadTripDTO> GetHistoryOfTripsByUserId(int userId)
		{
			_unitOfWork.Users.ThrowErrorIfNotExists(userId);

			IQueryable<Trip> rawTrips = _unitOfWork.Trips.GetHistoryOfTripsByUserId(userId);
			IQueryable<ReadTripDTO> mappedTrips = rawTrips.Select(el => _mapper.Map<ReadTripDTO>(el));
			return mappedTrips;
		}

		public IQueryable<ReadTripDTO> GetTripsByUserId(int userId)
		{
			_unitOfWork.Users.ThrowErrorIfNotExists(userId);

			IQueryable<Trip> rawTrips = _unitOfWork.Trips.GetTripsByUserId(userId);
			IQueryable<ReadTripDTO> mappedTrips = rawTrips.Select(el => _mapper.Map<ReadTripDTO>(el));
			return mappedTrips;
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

		public async Task<List<UtilDurationInMonth>> GetTotalDurationByMonthsAsync(int year, int userId)
		{
			List<Trip> trips = await _unitOfWork.Trips.GetTripsByYearAndUserId(year, userId).ToListAsync();

			if (trips.Count == 0)
			{
				return new();
			}

			List<UtilDurationInMonth> result = Enumerable.Range(1, 12)
				.Select(month => new UtilDurationInMonth()
				{
					Month = new DateTime(year, month, 1).ToString("MMMM"),
					TotalDuration = trips
						.Where(t => t.StartTime.Year == year && t.StartTime.Month <= month && t.EndTime.Year == year && t.EndTime.Month >= month)
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
}
