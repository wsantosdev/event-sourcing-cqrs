using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Positions.DomainEvents
{
    public class PositionOpenedHandler(IPositionReadModelStore store) : IMessageHandler<PositionOpened>
    {
        public async Task HandleAsync(PositionOpened @event) =>
            await store.StoreAsync(new PositionReadModel(@event.AccountId, @event.Symbol, @event.Available));
    }
}
