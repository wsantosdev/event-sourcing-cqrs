using WSantosDev.EventSourcing.Accounts.Commands;

namespace WSantosDev.EventSourcing.WebApi
{
    public sealed class DefaultHostedService(Open action) : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await action.ExecuteAsync(new OpenParams(Constants.DefaultAccountId, 1_000_000));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
