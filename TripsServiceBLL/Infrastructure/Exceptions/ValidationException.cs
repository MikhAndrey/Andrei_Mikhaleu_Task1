namespace TripsServiceBLL.Infrastructure.Exceptions;

public class ValidationException : Exception
{
	public ValidationException(string message, string prop) : base(message)
	{
		Property = prop;
	}

	public string Property { get; protected set; }
}
