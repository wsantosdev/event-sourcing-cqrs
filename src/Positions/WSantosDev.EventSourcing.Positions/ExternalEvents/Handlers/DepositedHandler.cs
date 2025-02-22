﻿using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Positions.ExternalEvents
{
    public class DepositedHandler(IPositionReadModelStore store) : IMessageHandler<PositionModified>
    {
        public void Handle(PositionModified @event)
        {
            var stored = store.GetBySymbol(@event.AccountId, @event.Symbol);
            var position = new PositionReadModel(@event.AccountId, @event.Symbol, @event.Available);

            if (stored)
            {
                store.Update(position);
                return;
            }

            store.Add(position);
        }   
    }
}
