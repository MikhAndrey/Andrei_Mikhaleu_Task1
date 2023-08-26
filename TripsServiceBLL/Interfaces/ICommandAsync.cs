namespace TripsServiceBLL.Interfaces;

public interface ICommandAsync<T>
{
	Task ExecuteAsync(T model);
}

public interface ICommandAsync<T, TResult>
{
	Task<TResult> ExecuteAsync(T model);
}
