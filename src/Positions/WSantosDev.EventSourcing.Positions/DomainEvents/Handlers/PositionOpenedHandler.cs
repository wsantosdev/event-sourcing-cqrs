using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Positions.DomainEvents
{
    public class PositionOpenedHandler(IPositionReadModelStore store) : IMessageHandler<PositionOpened>
    {
        public void Handle(PositionOpened @event) =>
            store.Store(new PositionReadModel(@event.AccountId, @event.Symbol, @event.Available));
    }
}
