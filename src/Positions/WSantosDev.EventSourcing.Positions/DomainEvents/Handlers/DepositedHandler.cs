using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Positions.DomainEvents
{
    public class DepositedHandler(IPositionReadModelStore store) : IMessageHandler<Deposited>
    {
        public async Task HandleAsync(Deposited @event) =>
            await store.StoreAsync(new PositionReadModel(@event.AccountId, @event.Symbol, @event.Available));
    }
}
