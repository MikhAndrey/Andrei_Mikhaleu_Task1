namespace TripsServiceBLL.Interfaces;

public interface ICommandAsync<T>
{
	Task ExecuteAsync(T model);
}
