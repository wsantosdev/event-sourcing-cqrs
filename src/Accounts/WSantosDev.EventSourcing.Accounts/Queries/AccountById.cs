using Moonad;
using WSantosDev.EventSourcing.Commons;

namespace WSantosDev.EventSourcing.Accounts.Queries
{
    public class AccountById(AccountViewDbContext dbContext)
    {
        public async Task<Option<AccountView>> ExecuteAsync(AccountByIdParams queryParams, CancellationToken cancellationToken = default) =>
            await dbContext.ByAccountIdAsync(queryParams.AccountId, cancellationToken);
    }

    public record AccountByIdParams(AccountId AccountId);
}
