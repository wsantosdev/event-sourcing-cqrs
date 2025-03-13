using Microsoft.EntityFrameworkCore;
using Moonad;
using WSantosDev.EventSourcing.Commons;

namespace WSantosDev.EventSourcing.Accounts
{
    public sealed class AccountViewDbContext(DbContextOptions<AccountViewDbContext> options) : DbContext(options)
    {
        private DbSet<AccountView> Accounts { get; set; }

        public async Task<Option<AccountView>> ByIdAsync(AccountId accountId, CancellationToken cancellationToken = default) =>
            (await Accounts.FirstOrDefaultAsync(a => a.AccountId == accountId, cancellationToken)).ToOption();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountView>()
                        .HasKey(a => a.AccountId);
            modelBuilder.Entity<AccountView>()
                        .Property(a => a.AccountId)
                        .HasConversion(v => v.ToString(), v => Guid.Parse(v));
            base.OnModelCreating(modelBuilder);
        }
    }
}
