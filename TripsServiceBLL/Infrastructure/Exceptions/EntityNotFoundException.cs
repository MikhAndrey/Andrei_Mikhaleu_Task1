namespace TripsServiceBLL.Infrastructure.Exceptions
{
	public class EntityNotFoundException : Exception
	{
		public EntityNotFoundException(string message) : base(message) { }
	}
}
