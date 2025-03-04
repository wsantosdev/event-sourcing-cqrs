using Moonad;
using WSantosDev.EventSourcing.Commons;

namespace WSantosDev.EventSourcing.Accounts.Queries
{
    public class AccountById(IAccountReadModelStore readModelStore)
    {
        public async Task<Option<AccountReadModel>> ExecuteAsync(AccountByIdParams queryParams) =>
            await readModelStore.ByIdAsync(queryParams.AccountId);
    }

    public record AccountByIdParams(AccountId AccountId);
}
