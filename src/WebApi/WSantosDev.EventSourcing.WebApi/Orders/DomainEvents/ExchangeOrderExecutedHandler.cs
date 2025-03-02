using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Exchange.DomainEvents;
using WSantosDev.EventSourcing.Orders.Actions;

namespace WSantosDev.EventSourcing.WebApi.Orders.DomainEvents
{
    public class ExchangeOrderExecutedHandler(ExecuteAction action) : IMessageHandler<ExchangeOrderExecuted>
    {
        private readonly IList<OrderId> _handledOrderIds = [];

        public void Handle(ExchangeOrderExecuted @event)
        {
            if (_handledOrderIds.Contains(@event.OrderId))
                return;

            action.ExecuteAsync(new ExecuteActionParams(@event.OrderId));

            _handledOrderIds.Add(@event.OrderId);
        }
    }
}
