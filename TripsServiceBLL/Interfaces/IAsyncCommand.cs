namespace TripsServiceBLL.Interfaces
{
	public interface IAsyncCommand
	{
		Task ExecuteAsync();
	}

	public interface IAsyncCommand<T> where T : class
	{
		Task<T> ExecuteAsync();
	}

	public interface ICommand<T> where T : class
	{
		T Execute();
	}
}
