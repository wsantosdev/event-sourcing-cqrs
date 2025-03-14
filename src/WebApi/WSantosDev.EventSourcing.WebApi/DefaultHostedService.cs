using WSantosDev.EventSourcing.Accounts.Commands;
using WSantosDev.EventSourcing.EventStore;

namespace WSantosDev.EventSourcing.WebApi
{
    public sealed class DefaultHostedService(EventDbContext eventDbContext, Open action) : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await action.ExecuteAsync(new OpenParams(Constants.DefaultAccountId, 1_000_000));
            EventIndex.Seed(eventDbContext.CurrentIndex());
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
