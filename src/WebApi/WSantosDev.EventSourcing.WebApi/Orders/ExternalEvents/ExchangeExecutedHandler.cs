using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Exchange.DomainEvents;
using WSantosDev.EventSourcing.Orders.Actions;

namespace WSantosDev.EventSourcing.WebApi.Orders.ExternalEvents
{
    public class ExchangeExecutedHandler(ExecuteAction action) : IMessageHandler<ExchangeExecuted>
    {
        private readonly IList<OrderId> _handledOrderIds = [];

        public void Handle(ExchangeExecuted @event)
        {
            if (_handledOrderIds.Contains(@event.OrderId))
                return;

            action.Execute(new ExecuteActionParams(@event.OrderId));

            _handledOrderIds.Add(@event.OrderId);
        }
    }
}
