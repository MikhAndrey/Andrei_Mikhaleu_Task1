using TripsServiceBLL.DTO.Feedbacks;

namespace TripsServiceBLL.Interfaces;

public interface IFeedbackService
{
	Task<ReadFeedbackDTO> AddAsync(CreateFeedbackDTO feedback);
	Task DeleteByTripIdAsync(int tripId);
	Task DeleteAsync(int id);
	Task UpdateAsync(UpdateFeedbackDTO dto);
}