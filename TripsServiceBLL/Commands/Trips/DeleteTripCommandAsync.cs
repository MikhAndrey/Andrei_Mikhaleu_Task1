using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using TripsServiceBLL.Helpers;
using TripsServiceBLL.Infrastructure.Exceptions;
using TripsServiceBLL.Interfaces;
using TripsServiceDAL.Interfaces;

namespace TripsServiceBLL.Commands.Trips
{
	public class DeleteTripCommandAsync : ICommandAsync<int>
	{

		private readonly ITripService _tripService;

		private readonly IImageService _imageService;

		private readonly IRoutePointService _routePointService;

		private readonly ICommentService _commentService;

		private readonly IFeedbackService _feedbackService;

		private readonly IWebHostEnvironment _env;

		private readonly IUnitOfWork _unitOfWork;

		private readonly IHttpContextAccessor _httpContextAccessor;

		public DeleteTripCommandAsync(
			ITripService tripService,
			IImageService imageService,
			IRoutePointService routePointService,
			ICommentService commentService,
			IFeedbackService feedbackService,
			IWebHostEnvironment env,
			IUnitOfWork unitOfWork,
			IHttpContextAccessor httpContextAccessor)
		{
			_tripService = tripService;
			_imageService = imageService;
			_routePointService = routePointService;
			_commentService = commentService;
			_feedbackService = feedbackService;
			_env = env;
			_unitOfWork = unitOfWork;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task ExecuteAsync(int id)
		{
			int userId = UserHelper.GetUserIdFromClaims(_httpContextAccessor.HttpContext.User.Claims);
			_unitOfWork.Users.ThrowErrorIfNotExists(userId);

			using (IDbContextTransaction transaction = _unitOfWork.BeginTransaction())
			{
				try
				{
					await _tripService.DeleteAsync(id);
					await _imageService.DeleteByTripIdAsync(id);
					await _routePointService.DeleteByTripIdAsync(id);
					await _commentService.DeleteByTripIdAsync(id);
					await _feedbackService.DeleteByTripIdAsync(id);
					await transaction.CommitAsync();
				}
				catch (Exception)
				{
					await transaction.RollbackAsync();
					throw new DbOperationException();
				}
			}

			//_imageService.DeleteTripImagesFiles(id, userId, _env.WebRootPath); TODO: Physical images removal is under discussion
		}
	}
}
