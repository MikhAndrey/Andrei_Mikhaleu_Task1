﻿using TripsServiceBLL.Services;
using TripsServiceDAL.Entities;
using TripsServiceBLL.Infrastructure;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.DTO.Statistics;

namespace TripsServiceBLL.Commands.Statistics
{
    public class GetDistinctTripYearsCommand : IAsyncCommand<YearsStatisticsDTO>
    {
        private ITripService _tripService;

        private IUserService _userService;

        private string _userName;

        public GetDistinctTripYearsCommand(ITripService tripService, IUserService userService, string userName)
        {
            _tripService = tripService;
            _userService = userService;
            _userName = userName;
        }

        public async Task<YearsStatisticsDTO> ExecuteAsync()
        {
            User? user = await _userService.GetByUserNameAsync(_userName);
            return _tripService.GetYearsOfUserTrips(user.UserId);
        }
    }
}
