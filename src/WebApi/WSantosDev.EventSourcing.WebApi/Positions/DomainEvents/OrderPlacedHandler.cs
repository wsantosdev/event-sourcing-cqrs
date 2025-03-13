using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Orders.DomainEvents;
using WSantosDev.EventSourcing.Positions.Commands;

namespace WSantosDev.EventSourcing.WebApi.Positions.DomainEvents
{
    public sealed class OrderPlacedHandler(Withdraw command) : IMessageHandler<OrderPlaced>
    {
        private readonly IList<OrderId> _handledOrderIds = [];

        public async Task HandleAsync(OrderPlaced @event)
        {
            if (@event.Side == OrderSide.Buy || _handledOrderIds.Contains(@event.OrderId))
                return;

            await command.ExecuteAsync(new WithdrawParams(@event.AccountId, @event.Symbol, @event.Quantity));
            _handledOrderIds.Add(@event.OrderId);
        }
    }
}
