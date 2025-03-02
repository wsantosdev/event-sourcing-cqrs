using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Exchange.DomainEvents
{
    public class ExchangeOrderCreatedHandler(IExchangeOrderReadModelStore readModelStore) : IMessageHandler<ExchangeOrderCreated>
    {
        public async Task HandleAsync(ExchangeOrderCreated @event)
        {
            await readModelStore.StoreAsync(new ExchangeOrderReadModel(@event.AccountId, @event.OrderId, @event.Side,
                                                                       @event.Quantity, @event.Symbol, @event.Price, @event.Status));
        }
    }
}
