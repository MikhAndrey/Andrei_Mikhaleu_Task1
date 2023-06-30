using AutoMapper;
using TripsServiceBLL.DTO.Feedbacks;
using TripsServiceBLL.Interfaces;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Infrastructure.Exceptions;
using TripsServiceDAL.Interfaces;

namespace TripsServiceBLL.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public FeedbackService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddAsync(CreateFeedbackDTO feedback)
        {
            _unitOfWork.Trips.ThrowErrorIfNotExists(feedback.TripId);

            Feedback feedbackToAdd = _mapper.Map<Feedback>(feedback);
            await _unitOfWork.Feedbacks.AddAsync(feedbackToAdd);
            await _unitOfWork.SaveAsync();
        }

		public async Task DeleteAsync(int id)
		{
			Feedback? feedback = await _unitOfWork.Feedbacks.GetByIdAsync(id);
			if (feedback == null)
			{
				throw new EntityNotFoundException(TripsServiceDAL.Utils.UtilConstants.GetEntityNotExistsMessage<Feedback>()());
			}
			_unitOfWork.Trips.ThrowErrorIfNotExists(feedback.TripId);
			_unitOfWork.Feedbacks.Delete(feedback);
			await _unitOfWork.SaveAsync();
		}

		public async Task DeleteByTripIdAsync(int tripId)
		{
			Feedback? feedback = await _unitOfWork.Feedbacks.GetByTripId(tripId);
			if (feedback != null)
			{
				_unitOfWork.Feedbacks.Delete(feedback);
				await _unitOfWork.SaveAsync();
			}
		}
	}
}
