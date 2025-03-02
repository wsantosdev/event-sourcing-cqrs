using Microsoft.EntityFrameworkCore;
using Moonad;
using WSantosDev.EventSourcing.Commons;

namespace WSantosDev.EventSourcing.Positions
{
    public class PositionReadModelStore(PositionReadModelDbContext context) : IPositionReadModelStore
    {
        public IEnumerable<PositionReadModel> GetByAccount(AccountId accountId) =>
            context.Positions.Where(p => p.AccountId == accountId);

        public Option<PositionReadModel> GetBySymbol(AccountId accountId, Symbol symbol) =>
            context.Positions.SingleOrDefault(p => p.AccountId == accountId && p.Symbol == symbol.Value)
                             .ToOption();

        public void Store(PositionReadModel position)
        {
            context.Add(position);
            context.SaveChanges();
        }

        public void Update(PositionReadModel position)
        {
            context.ChangeTracker.Clear();
            context.Update(position);
            context.SaveChanges();
        }

        public void Remove(PositionReadModel position)
        {
            context.ChangeTracker.Clear();
            context.Remove(position);
            context.SaveChanges();
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
