using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Orders.DomainEvents
{
    public class OrderPlacedHandler(IOrderReadModelStore store) : IMessageHandler<OrderPlaced>
    {
        public void Handle(OrderPlaced @event)
        {
            store.StoreAsync(new OrderReadModel(@event.AccountId, @event.OrderId, @event.Side,
                                           @event.Quantity, @event.Symbol, @event.Price, @event.Status));
        }
    }
}
