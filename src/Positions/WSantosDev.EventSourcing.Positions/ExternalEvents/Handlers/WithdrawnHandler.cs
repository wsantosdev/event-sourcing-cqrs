using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Positions.ExternalEvents
{
    public class WithdrawnHandler(IPositionReadModelStore store) : IMessageHandler<PositionModified>
    {
        public void Handle(PositionModified @event)
        {
            var position = new PositionReadModel(@event.AccountId, @event.Symbol, @event.Available);

            if(position.Available > 0)
                store.Update(position);

            if(position.Available == 0)
                store.Remove(position);
        }
    }
}
