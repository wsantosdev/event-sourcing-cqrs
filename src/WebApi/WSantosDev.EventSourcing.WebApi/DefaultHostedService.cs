using WSantosDev.EventSourcing.Accounts.Actions;

namespace WSantosDev.EventSourcing.WebApi
{
    public sealed class DefaultHostedService(OpenAction action) : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            action.Execute(new OpenActionParams(Constants.DefaultAccountId, 1_000_000));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
