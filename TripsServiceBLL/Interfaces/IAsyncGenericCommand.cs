namespace TripsServiceBLL.Interfaces
{
    public interface IAsyncGenericCommand<T> where T : class
    {
        Task<T> ExecuteAsync();
    }
}
