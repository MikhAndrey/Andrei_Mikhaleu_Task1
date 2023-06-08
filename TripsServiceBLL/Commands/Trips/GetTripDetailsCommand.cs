using TripsServiceBLL.Services;
using TripsServiceBLL.Infrastructure;
using TripsServiceDAL.Entities;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.DTO.Trips;

namespace TripsServiceBLL.Commands.Trips
{
    public class GetTripDetailsCommand : IAsyncCommand<TripDTO>
    {
        private TripService _tripService;

        private UserService _userService;

        private int _id;

        private string _userName;

        public GetTripDetailsCommand(TripService tripService, UserService userService, int id, string userName)
        {
            _tripService = tripService;
            _userService = userService;
            _id = id;
            _userName = userName;
        }

        public async Task<TripDTO> ExecuteAsync()
        {
            User? user = await _userService.GetByUserNameAsync(_userName);
            return await _tripService.InitializeTripDTOAsync(_id, user.UserId);
        }
    }
}
