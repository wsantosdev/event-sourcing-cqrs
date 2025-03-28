using Microsoft.EntityFrameworkCore;
using Moonad;

namespace WSantosDev.EventSourcing.Positions
{
    public class PositionViewDbContext(DbContextOptions<PositionViewDbContext> options) : DbContext(options)
    {
        private DbSet<PositionView> Positions { get; set; }

        public async Task<IEnumerable<PositionView>> ByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default) 
        {
            try
            {
                return await Positions.Where(p => p.AccountId == accountId && p.Available > 0).ToListAsync(cancellationToken);
            }
            catch 
            {
                return [];
            }
        }
        
        public async Task<Option<PositionView>> BySymbolAsync(Guid accountId, string symbol, CancellationToken cancellationToken = default) =>
            (await Positions.SingleOrDefaultAsync(p => p.AccountId == accountId && p.Symbol == symbol, cancellationToken)).ToOption();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PositionView>()
                        .HasKey(p => new { p.AccountId, p.Symbol });
            modelBuilder.Entity<PositionView>()
                        .Property(p => p.AccountId)
                        .HasConversion(v => v.ToString(), v => Guid.Parse(v));

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            base.OnConfiguring(optionsBuilder);
        }
    }
}
