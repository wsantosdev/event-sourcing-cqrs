using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Modeling;
using WSantosDev.EventSourcing.Orders;

namespace WSantosDev.EventSourcing.WebApi.Orders
{
    public sealed class OrderStore(EventStore eventStore) : IOrderStore
    {
        Option<Order> IOrderStore.GetById(OrderId orderId)
        {
            var stream = eventStore.Load(StreamId(orderId));

            return stream.Any()
                ? Order.Restore(stream)
                : Option.None<Order>();
        }

        public Result<IError> Store(Order order) =>
            eventStore.Append(StreamId(order.OrderId), order.UncommittedEvents);
        
        private static string StreamId(OrderId orderId) =>
            $"Order_{orderId}";
    }
}
