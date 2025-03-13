using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Positions.Commands
{
    public class Withdraw(PositionStore store)
    {
        public async Task<Result<IError>> ExecuteAsync(WithdrawParams command, CancellationToken cancellationToken = default)
        {
            var stored = await store.BySymbolAsync(command.AccountId, command.Symbol, cancellationToken);
            if (stored)
            {
                var position = stored.Get();
                var withdrawn = position.Withdraw(command.Quantity);
                if (withdrawn)
                    await store.StoreAsync(position, cancellationToken);

                return withdrawn;
            }

            return CommandErrors.PositionNotFound;
        }
    }

    public record WithdrawParams(AccountId AccountId, Symbol Symbol, Quantity Quantity);
}
