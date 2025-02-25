using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Commons.Modeling;
using WSantosDev.EventSourcing.Positions;
using WSantosDev.EventSourcing.Positions.DomainEvents;


namespace WSantosDev.EventSourcing.Positions.Actions
{
    public class DepositAction(IPositionStore store, IMessageBus messageBus)
    {
        public Result<IError> Execute(DepositActionParams command)
        {
            var stored = store.GetBySymbol(command.AccountId, command.Symbol);
            if (stored)
            {
                var position = stored.Get();
                var deposited = position.Deposit(command.Quantity);
                if (deposited)
                {
                    store.Store(position);
                    messageBus.Publish(new PositionModified(command.AccountId, command.Symbol, position.Available));
                }

                return deposited;
            }

            var opened = Position.Open(command.AccountId, command.Symbol, command.Quantity);
            if (opened)
            {
                store.Store(opened);
                messageBus.Publish(new PositionOpened(command.AccountId, command.Symbol, command.Quantity));

                return Result<IError>.Ok();
            }

            return Result<IError>.Error(opened.ErrorValue);
        }
    }

    public record DepositActionParams(AccountId AccountId, Symbol Symbol, Quantity Quantity);
}
