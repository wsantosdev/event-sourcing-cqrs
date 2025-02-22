﻿using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Exchange.Actions;
using WSantosDev.EventSourcing.Orders.ExternalEvents;

namespace WSantosDev.EventSourcing.WebApi.Exchange.ExternalEvents
{
    public class OrderPlacedEventHandler(CreateAction action) : IMessageHandler<OrderPlaced>
    {
        private readonly IList<OrderId> _handledOrderIds = [];

        public void Handle(OrderPlaced @event)
        {
            if (_handledOrderIds.Contains(@event.OrderId))
                return;

            action.Execute(new CreateActionParams(@event.AccountId, @event.OrderId, @event.Side, 
                                            @event.Quantity, @event.Symbol, @event.Price));

            _handledOrderIds.Add(@event.OrderId);
        }
    }
}
