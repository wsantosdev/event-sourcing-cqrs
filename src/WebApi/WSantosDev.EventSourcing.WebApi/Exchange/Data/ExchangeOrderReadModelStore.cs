using Microsoft.EntityFrameworkCore;
using WSantosDev.EventSourcing.Exchange;

namespace WSantosDev.EventSourcing.WebApi.Exchange
{
    public class ExchangeOrderReadModelStore(ExchangeOrderReadModelDbContext context) : IOrderReadModelStore
    {
        public IEnumerable<OrderReadModel> GetAll() =>
            context.ExchangeOrders;

        public void Store(OrderReadModel order)
        {
            context.Add(order);
            context.SaveChanges();
        }

        public void Update(OrderReadModel order)
        {
            context.ChangeTracker.Clear();
            context.Update(order);
            context.SaveChanges();
        }
    }

    public class ExchangeOrderReadModelDbContext(DbContextOptions<ExchangeOrderReadModelDbContext> options) : DbContext(options)
    {
        public DbSet<OrderReadModel> ExchangeOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderReadModel>()
                        .HasKey(e => e.OrderId);

            modelBuilder.Entity<OrderReadModel>()
                        .Property(e => e.AccountId)
                        .HasConversion(v => v.ToString(), v => Guid.Parse(v));
            modelBuilder.Entity<OrderReadModel>()
                        .Property(e => e.OrderId)
                        .HasConversion(v => v.ToString(), v => Guid.Parse(v));
            modelBuilder.Entity<OrderReadModel>()
                        .Property(e => e.Side)
                        .HasConversion(v => v.ToString(), v => v);
            modelBuilder.Entity<OrderReadModel>()
                        .Property(e => e.Status)
                        .HasConversion(v => v.ToString(), v => v);


            base.OnModelCreating(modelBuilder);
        }
    }
}
