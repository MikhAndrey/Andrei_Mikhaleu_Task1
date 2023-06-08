using TripsServiceBLL.Utils;
using TripsServiceBLL.Infrastructure;
using TripsServiceBLL.Services;
using TripsServiceDAL.Entities;
using TripsServiceBLL.Interfaces;

namespace TripsServiceBLL.Commands.Statistics
{
    public class GetTripDurationsByYearCommand : IAsyncGenericCommand<List<DurationInMonth>>
    {
        private TripService _tripService;

        private UserService _userService;

        private string _userName;

        private int _year;

        public GetTripDurationsByYearCommand(TripService tripService, UserService userService, string userName, int year)
        {
            _tripService = tripService;
            _userService = userService;
            _userName = userName;
            _year = year;
        }

        public async Task<List<DurationInMonth>> ExecuteAsync()
        {
            User? user = await _userService.GetByUserNameAsync(_userName);
            if (user == null)
                throw new ValidationException("User was not found", "");
            List<DurationInMonth> durations = await _tripService.GetTotalDurationByMonthsAsync(_year, user.UserId);
            return durations;
        }
    }
}
