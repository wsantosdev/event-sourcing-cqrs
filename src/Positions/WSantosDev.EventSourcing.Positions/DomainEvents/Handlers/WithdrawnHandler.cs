using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Positions.DomainEvents
{
    public class WithdrawnHandler(IPositionReadModelStore store) : IMessageHandler<PositionModified>
    {
        public async Task HandleAsync(PositionModified @event)
        {
            var position = new PositionReadModel(@event.AccountId, @event.Symbol, @event.Available);

            if(position.Available > 0)
                await store.StoreAsync(position);

            if(position.Available == 0)
                await store.RemoveAsync(position);
        }
    }
}
