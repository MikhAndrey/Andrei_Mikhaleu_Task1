using TripsServiceBLL.Utils;

namespace TripsServiceBLL.Infrastructure.Exceptions
{
    public class DbOperationException : Exception
    {
        public DbOperationException(string message = Constants.DefaultDatabaseExceptionMessage) : base(message) { }
    }
}
