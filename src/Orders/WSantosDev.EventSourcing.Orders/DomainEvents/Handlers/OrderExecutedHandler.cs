using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Orders.DomainEvents
{
    public class OrderExecutedHandler(IOrderReadModelStore store) : IMessageHandler<OrderExecuted>
    {
        public void Handle(OrderExecuted @event)
        {
            store.StoreAsync(new OrderReadModel(@event.AccountId, @event.OrderId, @event.Side,
                                           @event.Quantity, @event.Symbol, @event.Price, @event.Status));
        }
    }
}