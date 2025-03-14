using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Exchange.DomainEvents;
using WSantosDev.EventSourcing.Orders.Commands;

namespace WSantosDev.EventSourcing.WebApi.Orders.DomainEvents
{
    public class ExchangeOrderExecutedHandler(Execute action) : IMessageHandler<ExchangeOrderExecuted>
    {
        private readonly IList<OrderId> _handledOrderIds = [];

        public async Task HandleAsync(ExchangeOrderExecuted @event)
        {
            if (_handledOrderIds.Contains(@event.OrderId))
                return;

            await action.ExecuteAsync(new ExecuteActionParams(@event.OrderId));

            _handledOrderIds.Add(@event.OrderId);
        }
    }
}
