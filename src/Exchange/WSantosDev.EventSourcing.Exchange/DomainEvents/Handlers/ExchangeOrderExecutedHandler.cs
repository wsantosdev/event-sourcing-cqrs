using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Exchange.DomainEvents
{
    public class ExchangeOrderExecutedHandler(IExchangeOrderReadModelStore readModelStore) : IMessageHandler<ExchangeOrderExecuted>
    {
        public async Task HandleAsync(ExchangeOrderExecuted @event)
        {
            await readModelStore.StoreAsync(new ExchangeOrderReadModel(@event.AccountId, @event.OrderId, @event.Side,
                                                                       @event.Quantity, @event.Symbol, @event.Price, @event.Status));
        }
    }
}
