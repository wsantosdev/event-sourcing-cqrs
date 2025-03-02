using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Exchange.DomainEvents
{
    public class ExchangeOrderExecutedHandler(IExchangeOrderReadModelStore readModelStore) : IMessageHandler<ExchangeOrderExecuted>
    {
        public void Handle(ExchangeOrderExecuted @event)
        {
            readModelStore.StoreAsync(new ExchangeOrderReadModel(@event.AccountId, @event.OrderId, @event.Side,
                                                            @event.Quantity, @event.Symbol, @event.Price, @event.Status));
        }
    }
}
