using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Positions.Commands
{
    public class Deposit(PositionStore store)
    {
        public async Task<Result<IError>> ExecuteAsync(DepositParams command, CancellationToken cancellationToken = default)
        {
            var stored = await store.BySymbolAsync(command.AccountId, command.Symbol, cancellationToken);
            if (stored)
            {
                var position = stored.Get();
                var deposited = position.Deposit(command.Quantity);
                if (deposited)
                    await store.StoreAsync(position, cancellationToken);

                return deposited;
            }

            var opened = Position.Open(command.AccountId, command.Symbol, command.Quantity);
            if (opened)
            {
                await store.StoreAsync(opened, cancellationToken);
                return true;
            }

            return Result<IError>.Error(opened.ErrorValue);
        }
    }

    public record DepositParams(AccountId AccountId, Symbol Symbol, Quantity Quantity);
}
