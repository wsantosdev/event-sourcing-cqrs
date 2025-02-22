﻿using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Positions.ExternalEvents
{
    public class PositionOpenedHandler(IPositionReadModelStore store) : IMessageHandler<PositionOpened>
    {
        public void Handle(PositionOpened @event) =>
            store.Add(new PositionReadModel(@event.AccountId, @event.Symbol, @event.Available));
    }
}
