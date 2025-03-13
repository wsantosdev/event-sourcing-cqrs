using Microsoft.EntityFrameworkCore;
using Moonad;
using WSantosDev.EventSourcing.Commons;

namespace WSantosDev.EventSourcing.Accounts.Test
{
    public class AccountViewStore(AccountViewDbContext dbContext)
    {
        public async Task<Option<AccountView>> ByIdAsync(AccountId accountId) =>
            await dbContext.ByIdAsync(accountId);

        public async Task StoreAsync(AccountView account)
        {
            var stored = await dbContext.ByIdAsync(account.AccountId);
            if (stored)
            {
                dbContext.Entry(stored.Get()).State = EntityState.Detached;
                dbContext.Entry(account).State = EntityState.Modified;

                await dbContext.SaveChangesAsync();
                return;
            }

            dbContext.Add(account);
            await dbContext.SaveChangesAsync();
        }
    }
}
