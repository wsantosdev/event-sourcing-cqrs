using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Exchange.DomainEvents;
using WSantosDev.EventSourcing.Positions.Actions;

namespace WSantosDev.EventSourcing.WebApi.Positions.DomainEvents
{
    public class ExchangeOrderExecutedHandler(DepositAction depositAction) : IMessageHandler<ExchangeOrderExecuted>
    {
        private readonly IList<OrderId> _handledOrderIds = [];

        public void Handle(ExchangeOrderExecuted @event)
        {
            if (@event.Side == OrderSide.Sell || _handledOrderIds.Contains(@event.OrderId))
                return;

            if (@event.Side == OrderSide.Buy)
                depositAction.Execute(new DepositActionParams(@event.AccountId, @event.Symbol, @event.Quantity));

            _handledOrderIds.Add(@event.OrderId);
        }
    }
}
