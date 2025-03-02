using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Commons.Modeling;
using WSantosDev.EventSourcing.Positions.DomainEvents;

namespace WSantosDev.EventSourcing.Positions.Actions
{
    public class WithdrawAction(IPositionStore store, IMessageBus messageBus)
    {
        public async Task<Result<IError>> ExecuteAsync(WithdrawActionParams command)
        {
            var stored = await store.GetBySymbolAsync(command.AccountId, command.Symbol);
            if (stored)
            {
                var position = stored.Get();
                var withdrawn = position.Withdraw(command.Quantity);
                if (withdrawn)
                {
                    await store.StoreAsync(position);
                    messageBus.Publish(new PositionModified(command.AccountId, command.Symbol, position.Available));
                }

                return withdrawn;
            }

            return ActionErrors.PositionNotFound;
        }
    }

    public record WithdrawActionParams(AccountId AccountId, Symbol Symbol, Quantity Quantity);
}
