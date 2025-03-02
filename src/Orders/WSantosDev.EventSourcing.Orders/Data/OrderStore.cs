using System.Linq;
using System.Threading.Tasks;
using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Orders
{
    public sealed class OrderStore(EventStore eventStore) : IOrderStore
    {
        public async Task<Option<Order>> GetByIdAsync(OrderId orderId)
        {
            var stream = await eventStore.ReadAsync(StreamId(orderId));

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
