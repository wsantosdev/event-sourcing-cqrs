using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Orders.DomainEvents;
using WSantosDev.EventSourcing.Positions.Commands;

namespace WSantosDev.EventSourcing.WebApi.Positions.DomainEvents
{
    public class OrderExecutedHandler(Deposit command) : IMessageHandler<OrderExecuted>
    {
        private readonly IList<OrderId> _handledOrderIds = [];

        public async Task HandleAsync(OrderExecuted @event)
        {
            if (@event.Side == OrderSide.Sell || _handledOrderIds.Contains(@event.OrderId))
                return;

            await command.ExecuteAsync(new DepositParams(@event.AccountId, @event.Symbol, @event.Quantity));

            _handledOrderIds.Add(@event.OrderId);
        }
    }
}
