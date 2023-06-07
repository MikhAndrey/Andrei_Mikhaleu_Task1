using TripsServiceBLL.DTO;
using TripsServiceBLL.Services;
using TripsServiceDAL.Entities;
using TripsServiceBLL.Infrastructure;

namespace TripsServiceBLL.Commands.Statistics
{
    public class GetDistinctTripYearsCommand : IAsyncGenericCommand<YearsStatisticsDTO>
    {
        private TripService _tripService;

        private UserService _userService;

        private string _userName;

        public GetDistinctTripYearsCommand(TripService tripService, UserService userService, string userName)
        {
            _tripService = tripService;
            _userService = userService;
            _userName = userName;
        }
        
        public async Task<YearsStatisticsDTO> ExecuteAsync()
        {
            User? user = await _userService.GetByUserNameAsync(_userName);
            if (user == null)
                throw new ValidationException("User was not found", "");
            IQueryable<int> years = _tripService.GetYearsOfUserTrips(user.UserId);
            return new()
            {
                Years = years,
                SelectedYear = years.FirstOrDefault()
            };
        }
    }
}
