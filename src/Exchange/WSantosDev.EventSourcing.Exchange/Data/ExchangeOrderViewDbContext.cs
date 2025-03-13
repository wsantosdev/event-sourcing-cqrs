using Microsoft.EntityFrameworkCore;

namespace WSantosDev.EventSourcing.Exchange
{
    public class ExchangeOrderViewDbContext(DbContextOptions<ExchangeOrderViewDbContext> options) : DbContext(options)
    {
        public DbSet<ExchangeOrderView> ExchangeOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExchangeOrderView>()
                        .HasKey(e => e.OrderId);

            modelBuilder.Entity<ExchangeOrderView>()
                        .Property(e => e.AccountId)
                        .HasConversion(v => v.ToString(), v => Guid.Parse(v));
            modelBuilder.Entity<ExchangeOrderView>()
                        .Property(e => e.OrderId)
                        .HasConversion(v => v.ToString(), v => Guid.Parse(v));
            modelBuilder.Entity<ExchangeOrderView>()
                        .Property(e => e.Side)
                        .HasConversion(v => v.ToString(), v => v);
            modelBuilder.Entity<ExchangeOrderView>()
                        .Property(e => e.Status)
                        .HasConversion(v => v.ToString(), v => v);

            base.OnModelCreating(modelBuilder);
        }
    }
}
