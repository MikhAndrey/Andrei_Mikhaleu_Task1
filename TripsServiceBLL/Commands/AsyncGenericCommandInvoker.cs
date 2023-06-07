namespace TripsServiceBLL.Commands
{
    public class AsyncGenericCommandInvoker<T> where T : class
    {
        public IAsyncGenericCommand<T> Command { get; set; }

        public async Task<T> ExecuteCommandAsync()
        {
            return await Command.ExecuteAsync();
        }
    }
}
