using Microsoft.EntityFrameworkCore;
using Moonad;
using WSantosDev.EventSourcing.Commons;

namespace WSantosDev.EventSourcing.Accounts
{
    public class AccountReadModelStore(AccountReadModelDbContext context) : IAccountReadModelStore
    {
        public Option<AccountReadModel> GetById(AccountId accountId) =>
            context.Accounts.Where(a => a.AccountId == accountId)
                            .FirstOrDefault()
                            .ToOption();

        public void Store(AccountReadModel account)
        {
            context.Add(account);
            context.SaveChanges();
        }

        public void Update(AccountReadModel account)
        {
            context.ChangeTracker.Clear();
            context.Update(account);
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
