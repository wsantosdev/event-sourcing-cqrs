using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Orders.ExternalEvents
{
    public class OrderPlacedHandler(IOrderReadModelStore store) : IMessageHandler<OrderPlaced>
    {
        public void Handle(OrderPlaced @event)
        {
            store.Store(new OrderReadModel(@event.AccountId, @event.OrderId, @event.Side,
                                           @event.Quantity, @event.Symbol, @event.Price, @event.Status));
        }
    }
}
