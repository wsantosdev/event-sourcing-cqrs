using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Orders.DomainEvents;
using WSantosDev.EventSourcing.Accounts.Actions;

namespace WSantosDev.EventSourcing.WebApi.Accounts.DomainEvents
{
    public sealed class OrderPlacedHandler(DebitAction action) : IMessageHandler<OrderPlaced>
    {
        private readonly IList<OrderId> _handledOrderIds = [];

        public async Task HandleAsync(OrderPlaced @event)
        {
            if (@event.Side == OrderSide.Sell || _handledOrderIds.Contains(@event.OrderId))
                return;

            await action.ExecuteAsync(new DebitActionParams(@event.AccountId, @event.Quantity * @event.Price));
            _handledOrderIds.Add(@event.OrderId);
        }
    }
}
