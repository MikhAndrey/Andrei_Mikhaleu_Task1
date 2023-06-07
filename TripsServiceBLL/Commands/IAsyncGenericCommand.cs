namespace TripsServiceBLL.Commands
{
    public interface IAsyncGenericCommand<T> where T : class
    {
        Task<T> ExecuteAsync();
    }
}
