using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Exchange.DomainEvents
{
    public class ExchangeOrderExecutedHandler(IExchangeOrderReadModelStore readModelStore) : IMessageHandler<ExchangeOrderExecuted>
    {
        public void Handle(ExchangeOrderExecuted @event)
        {
            readModelStore.Update(new OrderReadModel(@event.AccountId, @event.OrderId, @event.Side,
                                                     @event.Quantity, @event.Symbol, @event.Price, @event.Status));
        }
    }
}
