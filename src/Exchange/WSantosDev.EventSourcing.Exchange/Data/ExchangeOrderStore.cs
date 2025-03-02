using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Exchange
{
    public sealed class ExchangeOrderStore(EventStore eventStore) : IExchangeOrderStore
    {
            public Option<ExchangeOrder> GetById(OrderId orderId)
            {
                var stream = eventStore.Load(StreamId(orderId));

                return stream.Any()
                    ? ExchangeOrder.Restore(stream)
                    : Option.None<ExchangeOrder>();
            }
            
            public async Task<Result<IError>> StoreAsync(ExchangeOrder order) =>
                await eventStore.AppendAsync(StreamId(order.OrderId), order.UncommittedEvents);
                

            private static string StreamId(OrderId orderId) =>
                $"ExchangeOrder_{orderId}";
    }
}

