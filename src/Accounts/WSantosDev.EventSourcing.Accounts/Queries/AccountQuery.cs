using Moonad;
using WSantosDev.EventSourcing.Commons;

namespace WSantosDev.EventSourcing.Accounts.Queries
{
    public class AccountQuery(IAccountReadModelStore readModelStore)
    {
        public async Task<Option<AccountReadModel>> ExecuteAsync(AccountQueryParams queryParams) =>
            await readModelStore.GetByIdAsync(queryParams.AccountId);
    }

    public record AccountQueryParams(AccountId AccountId);
}
