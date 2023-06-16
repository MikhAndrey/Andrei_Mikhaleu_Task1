using TripsServiceBLL.DTO.Feedbacks;

namespace TripsServiceBLL.Interfaces
{
	public interface IFeedbackService
	{
		Task AddAsync(CreateFeedbackDTO feedback);
	}
}
