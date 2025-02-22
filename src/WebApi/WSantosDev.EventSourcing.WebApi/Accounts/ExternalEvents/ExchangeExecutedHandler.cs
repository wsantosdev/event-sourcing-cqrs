﻿using WSantosDev.EventSourcing.Accounts.Actions;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Exchange.ExternalEvents;

namespace WSantosDev.EventSourcing.WebApi.Accounts.ExternalEvents
{
    public sealed class ExchangeExecutedHandler(CreditAction action) : IMessageHandler<ExchangeExecuted>
    {
        private readonly IList<OrderId> _handledOrderIds = [];

        public void Handle(ExchangeExecuted @event)
        {
            if (@event.Side == OrderSide.Buy || _handledOrderIds.Contains(@event.OrderId))
                return;

            action.Execute(new CreditActionParams(@event.AccountId, @event.Quantity * @event.Price));
            
            _handledOrderIds.Add(@event.OrderId);
        }
    }
}
