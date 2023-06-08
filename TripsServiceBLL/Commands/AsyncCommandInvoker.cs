using TripsServiceBLL.Interfaces;

namespace TripsServiceBLL.Commands
{
    public class AsyncCommandInvoker
    {
        public IAsyncCommand Command { get; set; }

        public async Task ExecuteCommandAsync()
        {
            await Command.ExecuteAsync();
        }
    }
}
