using WSantosDev.EventSourcing.Accounts.Commands;

namespace WSantosDev.EventSourcing.WebApi
{
    public sealed class DefaultHostedService(Open action) : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            action.ExecuteAsync(new OpenActionParams(Constants.DefaultAccountId, 1_000_000));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
