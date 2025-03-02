using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moonad;
using WSantosDev.EventSourcing.Commons;

namespace WSantosDev.EventSourcing.Orders
{
    public class OrderReadModelStore(OrderReadModelDbContext context) : IOrderReadModelStore
    {
        public async Task<IEnumerable<OrderReadModel>> GetByAccountAsync(AccountId accountId) =>
            await context.Orders
                         .Where(o => o.AccountId == accountId).ToListAsync();

        public async Task StoreAsync(OrderReadModel order)
        {
            var stored = context.Orders.FirstOrDefault(o => o.OrderId == order.OrderId).ToOption();
            if (stored)
            {
                context.ChangeTracker.Clear();
                context.Update(order);
                await context.SaveChangesAsync();
                return;
            }

            context.Add(order);
            await context.SaveChangesAsync();
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
