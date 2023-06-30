using Microsoft.AspNetCore.SignalR;
using TripsServiceBLL.DTO.Feedbacks;

namespace Andrei_Mikhaleu_Task1.Hubs
{
    public class FeedbacksHub : Hub
    {
        public async Task Delete(int id)
        {
            await Clients.All.SendAsync("FeedbackDelete", id);
        }

        public async Task Create(CreateFeedbackDTO feedback)
        {
            await Clients.All.SendAsync("FeedbackCreate", feedback);
        }
    }
}
