using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using TripsServiceBLL.DTO.Statistics;
using TripsServiceBLL.DTO.Trips;
using TripsServiceBLL.DTO.Users;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Utils;
using TripsServiceDAL.Entities;
using TripsServiceBLL.Infrastructure;
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
            Trip? trip = await GetByIdAsync(tripId);
            if (trip == null)
                throw new EntityNotFoundException(Constants.TripNotExistsMessage);
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
            Trip? trip = await GetByIdAsync(tripId);
			if (trip == null)
				throw new EntityNotFoundException(Constants.TripNotExistsMessage);
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
            if (trip == null)
                throw new EntityNotFoundException(Constants.TripNotExistsMessage);
			bool userExists = _unitOfWork.Users.Exists(userId);
			if (!userExists)
                throw new EntityNotFoundException(Constants.UserNotFoundMessage);
            return _mapper.Map<Trip, TripDetailsDTO>(trip, opt =>
				opt.AfterMap((src, dest) => dest.IsCurrentUserTrip = src.User.Id == userId));
		}

		public async Task<EditTripDTO> GetTripForEditingAsync(int tripId)
		{
			Trip? trip = await GetByIdAsync(tripId);
            if (trip == null)
                throw new EntityNotFoundException(Constants.TripNotExistsMessage);
            return _mapper.Map<EditTripDTO>(trip);
		}

		public IQueryable<ReadTripDTOExtended> GetOthersPublicTrips(int userId)
		{
            bool userExists = _unitOfWork.Users.Exists(userId);
            if (!userExists)
                throw new EntityNotFoundException(Constants.UserNotFoundMessage);
            IQueryable<Trip> rawTrips = _unitOfWork.Trips.GetOthersPublicTrips(userId);
			return rawTrips.Select(el => _mapper.Map<ReadTripDTOExtended>(el));
		}

		public IQueryable<ReadTripDTO> GetHistoryOfTripsByUserId(int userId)
		{
            bool userExists = _unitOfWork.Users.Exists(userId);
            if (!userExists)
                throw new EntityNotFoundException(Constants.UserNotFoundMessage);
            IQueryable<Trip> rawTrips = _unitOfWork.Trips.GetHistoryOfTripsByUserId(userId);
			return rawTrips.Select(el => _mapper.Map<ReadTripDTO>(el));
		}

		public IQueryable<ReadTripDTO> GetTripsByUserId(int userId)
		{
            bool userExists = _unitOfWork.Users.Exists(userId);
            if (!userExists)
                throw new EntityNotFoundException(Constants.UserNotFoundMessage);
            IQueryable<Trip> rawTrips = _unitOfWork.Trips.GetTripsByUserId(userId);
			return rawTrips.Select(el => _mapper.Map<ReadTripDTO>(el));
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
