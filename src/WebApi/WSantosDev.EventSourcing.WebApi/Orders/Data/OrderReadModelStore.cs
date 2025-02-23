using Microsoft.EntityFrameworkCore;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Orders;

namespace WSantosDev.EventSourcing.WebApi.Orders
{
    public class OrderReadModelStore(OrderReadModelDbContext context) : IOrderReadModelStore
    {
        public IEnumerable<OrderReadModel> GetByAccount(AccountId accountId) =>
            context.Orders.Where(o => o.AccountId == accountId);

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

    public class OrderReadModelDbContext(DbContextOptions<OrderReadModelDbContext> options) : DbContext(options)
    {
        public DbSet<OrderReadModel> Orders { get; set; }

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
                        .Property(e => e.Status)
                        .HasConversion(v => v.ToString(), v => v);

            base.OnModelCreating(modelBuilder);
        }
    }
}
