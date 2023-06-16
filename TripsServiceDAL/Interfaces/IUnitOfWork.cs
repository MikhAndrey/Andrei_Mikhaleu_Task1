namespace TripsServiceDAL.Interfaces
{
	public interface IUnitOfWork
	{
		ICommentRepository Comments { get; }

		IImageRepository Images { get; }

		IRoutePointRepository RoutePoints { get; }

		ITripRepository Trips { get; }

		IUserRepository Users { get; }

		IDriverRepository Drivers { get; }

		IFeedbackRepository Feedbacks { get; }

		Task SaveAsync();
	}
}
