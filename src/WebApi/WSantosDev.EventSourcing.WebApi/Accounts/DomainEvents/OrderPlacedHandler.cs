using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Orders.ExternalEvents;
using WSantosDev.EventSourcing.Accounts.Actions;

namespace WSantosDev.EventSourcing.WebApi.Accounts.DomainEvents
{
    public sealed class OrderPlacedHandler(DebitAction action) : IMessageHandler<OrderPlaced>
    {
        private readonly IList<OrderId> _handledOrderIds = [];

        public void Handle(OrderPlaced @event)
        {
            if (@event.Side == OrderSide.Sell || _handledOrderIds.Contains(@event.OrderId))
                return;

            action.Execute(new DebitActionParams(@event.AccountId, @event.Quantity * @event.Price));
            _handledOrderIds.Add(@event.OrderId);
        }
    }
}
