using TripsServiceBLL.Utils;

namespace TripsServiceBLL.Infrastructure.Exceptions
{
	public class DbOperationException : Exception
	{
		public DbOperationException(string message = UtilConstants.DefaultDatabaseExceptionMessage) : base(message) { }
	}
}
