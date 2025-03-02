using Microsoft.EntityFrameworkCore;
using Moonad;
using WSantosDev.EventSourcing.Commons;

namespace WSantosDev.EventSourcing.Positions
{
    public class PositionReadModelStore(PositionReadModelDbContext context) : IPositionReadModelStore
    {
        public async Task<IEnumerable<PositionReadModel>> GetByAccountAsync(AccountId accountId) =>
            await context.Positions.Where(p => p.AccountId == accountId).ToListAsync();

        public async Task<Option<PositionReadModel>> GetBySymbolAsync(AccountId accountId, Symbol symbol)
        {
            var stored = await context.Positions.SingleOrDefaultAsync(p => p.AccountId == accountId && p.Symbol == symbol.Value);
            return stored.ToOption();
        }
        
        public async Task StoreAsync(PositionReadModel position)
        {
            var stored = context.Positions
                                .FirstOrDefault(p => p.AccountId == position.AccountId && p.Symbol == position.Symbol)
                                .ToOption();

            if (stored)
            {
                context.ChangeTracker.Clear();
                context.Update(position);
                await context.SaveChangesAsync();
                return;
            }

            context.Add(position);
            await context.SaveChangesAsync();
        }

        public async Task RemoveAsync(PositionReadModel position)
        {
            context.ChangeTracker.Clear();
            context.Remove(position);
            await context.SaveChangesAsync();
        }
    }

    public class PositionReadModelDbContext(DbContextOptions<PositionReadModelDbContext> options) : DbContext(options)
    {
        public DbSet<PositionReadModel> Positions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PositionReadModel>()
                        .HasKey(p => new { p.AccountId, p.Symbol });
            modelBuilder.Entity<PositionReadModel>()
                        .Property(p => p.AccountId)
                        .HasConversion(v => v.ToString(), v => Guid.Parse(v));
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
