namespace TripsServiceBLL.Interfaces
{
    public interface ICommandAsync
    {
        Task ExecuteAsync();
    }

    public interface ICommandAsync<T>
    {
        Task ExecuteAsync(T model);
    }

	public interface ICommandAsync<T, TResult> where T : class where TResult : class
	{
		Task<T> ExecuteAsync(TResult model);
	}

	public interface ICommand<T> where T : class
    {
        T Execute();
    }
}
