using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Accounts.Commands
{
    public class Credit(AccountStore store)
    {
        public async Task<Result<IError>> ExecuteAsync(CreditParams @params)
        {
            var stored = await store.ByIdAsync(@params.AccountId);
            if (stored)
            {
                var account = stored.Get();
                var credited = account.Credit(@params.Amount);
                if (credited)
                    return await store.StoreAsync(account);
                
                return Result<IError>.Error(credited.ErrorValue);
            }

            return CommandErrors.AccountNotFound;
        }
    }

    public record CreditParams(AccountId AccountId, Money Amount);
}
