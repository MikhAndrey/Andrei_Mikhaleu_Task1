using AutoMapper;
using TripsServiceBLL.DTO.Feedbacks;
using TripsServiceBLL.Interfaces;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Interfaces;
using TripsServiceBLL.Infrastructure;
using TripsServiceBLL.Utils;

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
			bool tripExists = _unitOfWork.Trips.Exists(feedback.TripId);
			if (!tripExists)
				throw new EntityNotFoundException(Constants.TripNotExistsMessage);
			await _unitOfWork.Feedbacks.AddAsync(_mapper.Map<Feedback>(feedback));
			await _unitOfWork.SaveAsync();
		}
	}
}
