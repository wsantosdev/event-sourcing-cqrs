using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Exchange.ExternalEvents
{
    public class ExchangeExecutedHandler(IOrderReadModelStore readModelStore) : IMessageHandler<ExchangeExecuted>
    {
        public void Handle(ExchangeExecuted @event)
        {
            readModelStore.Update(new OrderReadModel(@event.AccountId, @event.OrderId, @event.Side,
                                                     @event.Quantity, @event.Symbol, @event.Price, @event.Status));
        }
    }
}
