using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Positions.DomainEvents
{
    public class DepositedHandler(IPositionReadModelStore store) : IMessageHandler<PositionModified>
    {
        public void Handle(PositionModified @event) =>
            store.StoreAsync(new PositionReadModel(@event.AccountId, @event.Symbol, @event.Available));
    }
}
