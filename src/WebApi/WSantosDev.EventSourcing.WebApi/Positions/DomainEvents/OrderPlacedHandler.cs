using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Orders.DomainEvents;
using WSantosDev.EventSourcing.Positions.Actions;

namespace WSantosDev.EventSourcing.WebApi.Positions.DomainEvents
{
    public sealed class OrderPlacedHandler(WithdrawAction action) : IMessageHandler<OrderPlaced>
    {
        private readonly IList<OrderId> _handledOrderIds = [];

        public void Handle(OrderPlaced @event)
        {
            if (@event.Side == OrderSide.Buy || _handledOrderIds.Contains(@event.OrderId))
                return;

            action.Execute(new WithdrawActionParams(@event.AccountId, @event.Symbol, @event.Quantity));
            _handledOrderIds.Add(@event.OrderId);
        }
    }
}
