using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Exchange.Commands;
using WSantosDev.EventSourcing.Orders.DomainEvents;

namespace WSantosDev.EventSourcing.WebApi.Exchange.DomainEvents
{
    public class OrderPlacedHandler(Create action) : IMessageHandler<OrderPlaced>
    {
        private readonly IList<OrderId> _handledOrderIds = [];

        public async Task HandleAsync(OrderPlaced @event)
        {
            if (_handledOrderIds.Contains(@event.OrderId))
                return;

            await action.ExecuteAsync(new CreateActionParams(@event.AccountId, @event.OrderId, @event.Side, 
                                            @event.Quantity, @event.Symbol, @event.Price));

            _handledOrderIds.Add(@event.OrderId);
        }
    }
}
