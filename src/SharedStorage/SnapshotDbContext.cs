using Microsoft.EntityFrameworkCore;
using Moonad;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.SharedStorage
{
    public sealed class SnapshotDbContext(DbContextOptions<SnapshotDbContext> options) : DbContext(options)
    {
        private DbSet<Snapshot> Snapshots { get; set; }

        public async Task<Option<ISnapshot>> ByIdAsync(string snapshotId, CancellationToken cancellationToken = default)
        {
            try
            {
                var snapshot = (await Snapshots.FirstOrDefaultAsync(e => e.Id == snapshotId, cancellationToken))
                                               .ToOption();
                
                return snapshot 
                        ? SnapshotSerializer.Deserialize(snapshot.Get()).ToOption()
                        : Option.None<ISnapshot>();
            }
            catch
            {
                return Option.None<ISnapshot>();
            }
        }

        public async Task UpsertAsync(string snapshotId, ISnapshot snapshot, CancellationToken cancellationToken = default)
        {
            var storedSnapshot = (await Snapshots.FirstOrDefaultAsync(s => s.Id == snapshotId, cancellationToken)).ToOption();
            if (storedSnapshot)
                Remove(storedSnapshot.Get());

            Snapshots.Add(SnapshotSerializer.Serialize(snapshotId, snapshot));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Snapshot>().HasKey(e => new { e.Type, e.EntityVersion });
            base.OnModelCreating(modelBuilder);
        }
    }

    internal record Snapshot(string Id, string Type, long EntityVersion, string Created, string Metadata, string Data);
}
