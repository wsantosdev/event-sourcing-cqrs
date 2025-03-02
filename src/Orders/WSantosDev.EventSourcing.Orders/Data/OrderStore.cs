using System.Linq;
using System.Threading.Tasks;
using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Orders
{
    public sealed class OrderStore(EventStore eventStore) : IOrderStore
    {
        public Option<Order> GetById(OrderId orderId)
        {
            var stream = eventStore.Load(StreamId(orderId));

            return stream.Any()
                ? Order.Restore(stream)
                : Option.None<Order>();
        }

        public async Task<Result<IError>> StoreAsync(Order order) =>
            await eventStore.AppendAsync(StreamId(order.OrderId), order.UncommittedEvents);
        
        private static string StreamId(OrderId orderId) =>
            $"Order_{orderId}";
    }
}
