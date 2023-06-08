using TripsServiceDAL.Entities;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.DTO.Trips;

namespace TripsServiceBLL.Commands.Trips
{
    public class GetTripDetailsCommand : IAsyncCommand<TripDTO>
    {
        private ITripService _tripService;

        private IUserService _userService;

        private int _id;

        private string _userName;

        public GetTripDetailsCommand(ITripService tripService, IUserService userService, int id, string userName)
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
