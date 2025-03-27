using Microsoft.EntityFrameworkCore;
using Moonad;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.SharedStorage
{
    public sealed class SnapshotDbContext(DbContextOptions<SnapshotDbContext> options) : DbContext(options)
    {
        private DbSet<Snapshot> Snapshots { get; set; }

        public async Task<Option<T>> ByIdAsync<T>(string snapshotId, CancellationToken cancellationToken = default) where T : notnull
        {
            try
            {
                var snapshot = (await Snapshots.FirstOrDefaultAsync(e => e.EntityId == snapshotId, cancellationToken)).ToOption();
                
                return snapshot 
                        ? SnapshotSerializer.Deserialize<T>(snapshot.Get())
                        : Option.None<T>();
            }
            catch
            {
                return Option.None<T>();
            }
        }

        public async Task UpsertAsync<T>(string entityId, T snapshot, CancellationToken cancellationToken = default) where T : ISnapshot
        {
            var storedSnapshot = (await Snapshots.FirstOrDefaultAsync(s => s.EntityId == entityId, cancellationToken)).ToOption();
            if (storedSnapshot)
                Remove(storedSnapshot.Get());

            Snapshots.Add(SnapshotSerializer.Serialize(entityId, snapshot));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Snapshot>().HasKey(e => new { e.EntityId, e.EntityVersion });
            base.OnModelCreating(modelBuilder);
        }
    }

    internal record Snapshot(string EntityId, long EntityVersion, string Created, string Data);
}
