using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moonad;
using WSantosDev.EventSourcing.Commons;

namespace WSantosDev.EventSourcing.Orders
{
    public class OrderViewDbContext(DbContextOptions<OrderViewDbContext> options) : DbContext(options)
    {
        private DbSet<OrderView> Orders { get; set; }

        public async Task<IEnumerable<OrderView>> ByAccountIdAsync(AccountId accountId, CancellationToken cancellationToken = default) =>
            await Orders.Where(o => o.AccountId == accountId)
                        .ToListAsync(cancellationToken);

        public async Task<Option<OrderView>> ByOrderIdAsync(OrderId orderId, CancellationToken cancellationToken = default) =>
            (await Orders.AsNoTracking().FirstOrDefaultAsync(o => o.OrderId == orderId, cancellationToken)).ToOption();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderView>()
                        .HasKey(e => e.OrderId);

            modelBuilder.Entity<OrderView>()
                        .Property(e => e.AccountId)
                        .HasConversion(v => v.ToString(), v => Guid.Parse(v));
            modelBuilder.Entity<OrderView>()
                        .Property(e => e.OrderId)
                        .HasConversion(v => v.ToString(), v => Guid.Parse(v));
            modelBuilder.Entity<OrderView>()
                        .Property(e => e.Side)
                        .HasConversion(v => v.ToString(), v => v);
            modelBuilder.Entity<OrderView>()
                        .Property(e => e.Status)
                        .HasConversion(v => v.ToString(), v => v);

            base.OnModelCreating(modelBuilder);
        }
    }
}
