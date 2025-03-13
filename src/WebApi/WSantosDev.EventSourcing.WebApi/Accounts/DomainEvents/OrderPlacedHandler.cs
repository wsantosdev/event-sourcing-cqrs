using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Orders.DomainEvents;
using WSantosDev.EventSourcing.Accounts.Commands;

namespace WSantosDev.EventSourcing.WebApi.Accounts.DomainEvents
{
    public sealed class OrderPlacedHandler(Debit action) : IMessageHandler<OrderPlaced>
    {
        private readonly IList<OrderId> _handledOrderIds = [];

        public async Task HandleAsync(OrderPlaced @event)
        {
            if (@event.Side == OrderSide.Sell || _handledOrderIds.Contains(@event.OrderId))
                return;

            await action.ExecuteAsync(new DebitParams(@event.AccountId, @event.Quantity * @event.Price));
            _handledOrderIds.Add(@event.OrderId);
        }
    }
}
