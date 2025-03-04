using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Commons.Modeling;
using WSantosDev.EventSourcing.Positions.DomainEvents;

namespace WSantosDev.EventSourcing.Positions.Commands
{
    public class Deposit(IPositionStore store, IMessageBus messageBus)
    {
        public async Task<Result<IError>> ExecuteAsync(DepositParams command)
        {
            var stored = await store.GetBySymbolAsync(command.AccountId, command.Symbol);
            if (stored)
            {
                var position = stored.Get();
                var deposited = position.Deposit(command.Quantity);
                if (deposited)
                {
                    await store.StoreAsync(position);
                    messageBus.Publish(new Deposited(command.AccountId, command.Symbol, position.Available));
                }

                return deposited;
            }

            var opened = Position.Open(command.AccountId, command.Symbol, command.Quantity);
            if (opened)
            {
                await store.StoreAsync(opened);
                messageBus.Publish(new PositionOpened(command.AccountId, command.Symbol, command.Quantity));

                return true;
            }

            return Result<IError>.Error(opened.ErrorValue);
        }
    }

    public record DepositParams(AccountId AccountId, Symbol Symbol, Quantity Quantity);
}
