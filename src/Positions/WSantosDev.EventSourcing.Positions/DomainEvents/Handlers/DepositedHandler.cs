using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Positions.DomainEvents
{
    public class DepositedHandler(IPositionReadModelStore store) : IMessageHandler<PositionModified>
    {
        public async Task HandleAsync(PositionModified @event) =>
            await store.StoreAsync(new PositionReadModel(@event.AccountId, @event.Symbol, @event.Available));
    }
}
