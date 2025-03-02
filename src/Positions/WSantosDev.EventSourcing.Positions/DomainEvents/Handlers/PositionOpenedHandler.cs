using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Positions.DomainEvents
{
    public class PositionOpenedHandler(IPositionReadModelStore store) : IMessageHandler<PositionOpened>
    {
        public void Handle(PositionOpened @event) =>
            store.StoreAsync(new PositionReadModel(@event.AccountId, @event.Symbol, @event.Available));
    }
}
