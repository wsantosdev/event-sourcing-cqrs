using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Exchange.DomainEvents
{
    public class ExchangeCreatedHandler(IExchangeOrderReadModelStore readModelStore) : IMessageHandler<ExchangeCreated>
    {
        public void Handle(ExchangeCreated @event)
        {
            readModelStore.Store(new OrderReadModel(@event.AccountId, @event.OrderId, @event.Side,
                                                    @event.Quantity, @event.Symbol, @event.Price, @event.Status));
        }
    }
}
