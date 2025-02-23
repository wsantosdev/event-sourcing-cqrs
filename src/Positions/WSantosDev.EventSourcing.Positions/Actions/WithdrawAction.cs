using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Commons.Modeling;
using WSantosDev.EventSourcing.Positions.ExternalEvents;

namespace WSantosDev.EventSourcing.Positions.Actions
{
    public class WithdrawAction(IPositionStore store, IMessageBus messageBus)
    {
        public Result<IError> Execute(WithdrawActionParams command)
        {
            var stored = store.GetBySymbol(command.AccountId, command.Symbol);
            if (stored)
            {
                var position = stored.Get();
                var withdrawn = position.Withdraw(command.Quantity);
                if (withdrawn)
                {
                    store.Store(position);
                    messageBus.Publish(new PositionModified(command.AccountId, command.Symbol, position.Available));
                }

                return withdrawn;
            }

            return ActionErrors.PositionNotFound;
        }
    }

    public record WithdrawActionParams(AccountId AccountId, Symbol Symbol, Quantity Quantity);
}
