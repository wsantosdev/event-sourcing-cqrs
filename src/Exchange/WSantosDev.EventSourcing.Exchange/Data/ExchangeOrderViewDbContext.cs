using Microsoft.EntityFrameworkCore;
using Moonad;

namespace WSantosDev.EventSourcing.Exchange
{
    public class ExchangeOrderViewDbContext(DbContextOptions<ExchangeOrderViewDbContext> options) : DbContext(options)
    {
        private DbSet<ExchangeOrderView> ExchangeOrders { get; set; }

        public async Task<IEnumerable<ExchangeOrderView>> ListAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await ExchangeOrders.ToListAsync(cancellationToken);
            }
            catch 
            {
                return [];
            }
        }

        public async Task<Option<ExchangeOrderView>> ByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await ExchangeOrders.FirstOrDefaultAsync(e => e.OrderId == orderId, cancellationToken);
            }
            catch 
            { 
                return Option.None<ExchangeOrderView>();
            }
        }

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
