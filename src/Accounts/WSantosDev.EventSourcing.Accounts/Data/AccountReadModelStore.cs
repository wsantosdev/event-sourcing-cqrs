using Microsoft.EntityFrameworkCore;
using Moonad;
using WSantosDev.EventSourcing.Commons;

namespace WSantosDev.EventSourcing.Accounts
{
    public class AccountReadModelStore(AccountReadModelDbContext context) : IAccountReadModelStore
    {
        public async Task<Option<AccountReadModel>> ByIdAsync(AccountId accountId)
        {
            var stored = await context.Accounts.FirstOrDefaultAsync(a => a.AccountId == accountId);
            return stored.ToOption();
        }

        public async Task StoreAsync(AccountReadModel account)
        {
            var stored = (await context.Accounts.FirstOrDefaultAsync(a => a.AccountId == account.AccountId)).ToOption();
            if (stored)
            {
                context.ChangeTracker.Clear();
                context.Update(account);
                context.SaveChanges();
                return;
            }

            context.Add(account);
            context.SaveChanges();
        }
    }

    public sealed class AccountReadModelDbContext(DbContextOptions<AccountReadModelDbContext> options) : DbContext(options)
    {
        public DbSet<AccountReadModel> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountReadModel>()
                        .HasKey(a => a.AccountId);
            modelBuilder.Entity<AccountReadModel>()
                        .Property(a => a.AccountId)
                        .HasConversion(v => v.ToString(), v => Guid.Parse(v));
            base.OnModelCreating(modelBuilder);
        }
    }
}
