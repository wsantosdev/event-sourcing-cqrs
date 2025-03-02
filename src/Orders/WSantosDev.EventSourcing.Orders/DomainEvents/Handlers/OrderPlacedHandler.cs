using System.Threading.Tasks;
using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Orders.DomainEvents
{
    public class OrderPlacedHandler(IOrderReadModelStore store) : IMessageHandler<OrderPlaced>
    {
        public async Task HandleAsync(OrderPlaced @event)
        {
            await store.StoreAsync(new OrderReadModel(@event.AccountId, @event.OrderId, @event.Side,
                                           @event.Quantity, @event.Symbol, @event.Price, @event.Status));
        }
    }
}
