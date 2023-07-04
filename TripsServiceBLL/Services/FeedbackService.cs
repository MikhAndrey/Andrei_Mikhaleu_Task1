using AutoMapper;
using TripsServiceBLL.DTO.Feedbacks;
using TripsServiceBLL.Interfaces;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Interfaces;

namespace TripsServiceBLL.Services;

public class FeedbackService : IFeedbackService
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;

	public FeedbackService(IUnitOfWork unitOfWork, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

	public async Task<ReadFeedbackDTO> AddAsync(CreateFeedbackDTO feedback)
	{
		_unitOfWork.Trips.ThrowErrorIfNotExists(feedback.TripId);

		Feedback feedbackToAdd = _mapper.Map<Feedback>(feedback);
		await _unitOfWork.Feedbacks.AddAsync(feedbackToAdd);
		await _unitOfWork.SaveAsync();
		ReadFeedbackDTO dto = _mapper.Map<ReadFeedbackDTO>(feedback);
		dto.Id = feedbackToAdd.Id;
		return dto;
	}

	public async Task UpdateAsync(UpdateFeedbackDTO dto)
	{
		Feedback feedback = await _unitOfWork.Feedbacks.GetByIdAsync(dto.Id);
		_unitOfWork.Trips.ThrowErrorIfNotExists(feedback.TripId);
		_mapper.Map(dto, feedback);
		_unitOfWork.Feedbacks.Update(feedback);
		await _unitOfWork.SaveAsync();
	}

	public async Task DeleteAsync(int id)
	{
		Feedback feedback = await _unitOfWork.Feedbacks.GetByIdAsync(id);
		_unitOfWork.Trips.ThrowErrorIfNotExists(feedback.TripId);
		_unitOfWork.Feedbacks.Delete(feedback);
		await _unitOfWork.SaveAsync();
	}

	public async Task DeleteByTripIdAsync(int tripId)
	{
		Feedback feedback = await _unitOfWork.Feedbacks.GetByTripId(tripId);
		_unitOfWork.Feedbacks.Delete(feedback);
		await _unitOfWork.SaveAsync();
	}
}