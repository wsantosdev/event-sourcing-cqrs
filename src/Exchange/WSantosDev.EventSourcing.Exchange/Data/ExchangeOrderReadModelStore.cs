using Microsoft.EntityFrameworkCore;
using Moonad;

namespace WSantosDev.EventSourcing.Exchange
{
    public class ExchangeOrderReadModelStore(ExchangeOrderReadModelDbContext context) : IExchangeOrderReadModelStore
    {
        public IEnumerable<ExchangeOrderReadModel> GetAll() =>
            context.ExchangeOrders;

        public void Store(ExchangeOrderReadModel order)
        {
            var stored = context.ExchangeOrders.FirstOrDefault(o => o.OrderId == order.OrderId).ToOption();
            if (stored)
            {
                context.ChangeTracker.Clear();
                context.Update(order);
                context.SaveChanges();
                return;
            }

            context.Add(order);
            context.SaveChanges();
        }
    }

    public class ExchangeOrderReadModelDbContext(DbContextOptions<ExchangeOrderReadModelDbContext> options) : DbContext(options)
    {
        public DbSet<ExchangeOrderReadModel> ExchangeOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExchangeOrderReadModel>()
                        .HasKey(e => e.OrderId);

            modelBuilder.Entity<ExchangeOrderReadModel>()
                        .Property(e => e.AccountId)
                        .HasConversion(v => v.ToString(), v => Guid.Parse(v));
            modelBuilder.Entity<ExchangeOrderReadModel>()
                        .Property(e => e.OrderId)
                        .HasConversion(v => v.ToString(), v => Guid.Parse(v));
            modelBuilder.Entity<ExchangeOrderReadModel>()
                        .Property(e => e.Side)
                        .HasConversion(v => v.ToString(), v => v);
            modelBuilder.Entity<ExchangeOrderReadModel>()
                        .Property(e => e.Status)
                        .HasConversion(v => v.ToString(), v => v);


            base.OnModelCreating(modelBuilder);
        }
    }
}
