using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Accounts.Commands
{
    public class Debit(AccountStore store)
    {
        public async Task<Result<IError>> ExecuteAsync(DebitParams @params)
        {
            var stored = await store.ByIdAsync(@params.AccountId);
            if (stored)
            {
                var account = stored.Get();
                var debited = account.Debit(@params.Amount);
                if (debited)
                    return await store.StoreAsync(account);

                return Result<IError>.Error(debited.ErrorValue);
            }

            return CommandErrors.AccountNotFound;
        }
    }

    public record DebitParams(AccountId AccountId, Money Amount);
}
