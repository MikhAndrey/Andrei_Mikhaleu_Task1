namespace TripsServiceBLL.Commands
{
    public interface IAsyncCommand
    {
        Task ExecuteAsync();
    }
}
