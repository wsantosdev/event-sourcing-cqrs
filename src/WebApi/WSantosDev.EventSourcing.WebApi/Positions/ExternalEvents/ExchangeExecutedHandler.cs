﻿using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Exchange.ExternalEvents;
using WSantosDev.EventSourcing.Positions.Actions;

namespace WSantosDev.EventSourcing.WebApi.Positions.ExternalEvents
{
    public class ExchangeExecutedHandler(DepositAction depositAction) : IMessageHandler<ExchangeExecuted>
    {
        private readonly IList<OrderId> _handledOrderIds = [];

        public void Handle(ExchangeExecuted @event)
        {
            if (@event.Side == OrderSide.Sell || _handledOrderIds.Contains(@event.OrderId))
                return;

            if (@event.Side == OrderSide.Buy)
                depositAction.Execute(new DepositActionParams(@event.AccountId, @event.Symbol, @event.Quantity));

            _handledOrderIds.Add(@event.OrderId);
        }
    }
}
