using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Exchange.DomainEvents
{
    public class ExchangeOrderCreatedHandler(IExchangeOrderReadModelStore readModelStore) : IMessageHandler<ExchangeOrderCreated>
    {
        public void Handle(ExchangeOrderCreated @event)
        {
            readModelStore.Store(new ExchangeOrderReadModel(@event.AccountId, @event.OrderId, @event.Side,
                                                    @event.Quantity, @event.Symbol, @event.Price, @event.Status));
        }
    }
}
