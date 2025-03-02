using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Positions.DomainEvents
{
    public class WithdrawnHandler(IPositionReadModelStore store) : IMessageHandler<PositionModified>
    {
        public void Handle(PositionModified @event)
        {
            var position = new PositionReadModel(@event.AccountId, @event.Symbol, @event.Available);

            if(position.Available > 0)
                store.StoreAsync(position);

            if(position.Available == 0)
                store.RemoveAsync(position);
        }
    }
}
