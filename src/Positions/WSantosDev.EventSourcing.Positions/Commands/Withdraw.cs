using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Commons.Modeling;
using WSantosDev.EventSourcing.Positions.DomainEvents;

namespace WSantosDev.EventSourcing.Positions.Commands
{
    public class Withdraw(IPositionStore store, IMessageBus messageBus)
    {
        public async Task<Result<IError>> ExecuteAsync(WithdrawParams command)
        {
            var stored = await store.GetBySymbolAsync(command.AccountId, command.Symbol);
            if (stored)
            {
                var position = stored.Get();
                var withdrawn = position.Withdraw(command.Quantity);
                if (withdrawn)
                {
                    await store.StoreAsync(position);
                    messageBus.Publish(new Withdrawn(command.AccountId, command.Symbol, position.Available));
                }

                return withdrawn;
            }

            return CommandErrors.PositionNotFound;
        }
    }

    public record WithdrawParams(AccountId AccountId, Symbol Symbol, Quantity Quantity);
}
