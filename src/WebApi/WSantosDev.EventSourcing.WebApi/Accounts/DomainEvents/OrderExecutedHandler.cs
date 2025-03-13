using WSantosDev.EventSourcing.Accounts.Commands;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Orders.DomainEvents;

namespace WSantosDev.EventSourcing.WebApi.Accounts.DomainEvents
{
    public sealed class OrderExecutedHandler(Credit action) : IMessageHandler<OrderExecuted>
    {
        private readonly IList<OrderId> _handledOrderIds = [];

        public async Task HandleAsync(OrderExecuted @event)
        {
            if (@event.Side == OrderSide.Buy || _handledOrderIds.Contains(@event.OrderId))
                return;

            await action.ExecuteAsync(new CreditParams(@event.AccountId, @event.Quantity * @event.Price));
            
            _handledOrderIds.Add(@event.OrderId);
        }
    }
}
