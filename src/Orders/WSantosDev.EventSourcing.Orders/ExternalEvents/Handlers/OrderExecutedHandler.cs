using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Orders.ExternalEvents
{
    public class OrderExecutedHandler(IOrderReadModelStore store) : IMessageHandler<OrderExecuted>
    {
        public void Handle(OrderExecuted @event)
        {
            store.Update(new OrderReadModel(@event.AccountId, @event.OrderId, @event.Side,
                                            @event.Quantity, @event.Symbol, @event.Price, @event.Status));
        }
    }
}