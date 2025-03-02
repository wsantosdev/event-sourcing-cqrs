using WSantosDev.EventSourcing.Accounts.Actions;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Exchange.DomainEvents;

namespace WSantosDev.EventSourcing.WebApi.Accounts.DomainEvents
{
    public sealed class ExchangeOrderExecutedHandler(CreditAction action) : IMessageHandler<ExchangeOrderExecuted>
    {
        private readonly IList<OrderId> _handledOrderIds = [];

        public async Task HandleAsync(ExchangeOrderExecuted @event)
        {
            if (@event.Side == OrderSide.Buy || _handledOrderIds.Contains(@event.OrderId))
                return;

            await action.ExecuteAsync(new CreditActionParams(@event.AccountId, @event.Quantity * @event.Price));
            
            _handledOrderIds.Add(@event.OrderId);
        }
    }
}
