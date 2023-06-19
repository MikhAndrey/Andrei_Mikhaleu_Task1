namespace TripsServiceBLL.Interfaces
{
    public interface ICommandAsync
    {
        Task ExecuteAsync();
    }

    public interface ICommandAsync<T> where T : class
    {
        Task<T> ExecuteAsync();
    }

    public interface ICommand<T> where T : class
    {
        T Execute();
    }
}
