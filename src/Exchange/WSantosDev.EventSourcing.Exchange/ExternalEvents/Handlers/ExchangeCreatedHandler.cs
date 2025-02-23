using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Exchange.ExternalEvents
{
    public class ExchangeCreatedHandler(IOrderReadModelStore readModelStore) : IMessageHandler<ExchangeCreated>
    {
        public void Handle(ExchangeCreated @event)
        {
            readModelStore.Store(new OrderReadModel(@event.AccountId, @event.OrderId, @event.Side,
                                                    @event.Quantity, @event.Symbol, @event.Price, @event.Status));
        }
    }
}
