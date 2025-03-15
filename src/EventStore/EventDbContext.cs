using Microsoft.EntityFrameworkCore;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.EventStore
{
    public sealed class EventDbContext(DbContextOptions<EventDbContext> options) : DbContext(options)
    {
        private DbSet<Event> Events { get; set; }

        public async Task<IEnumerable<IEvent>> ReadStreamAsync(string streamId, CancellationToken cancellationToken = default)
        {
            try
            {
                var stream = await Events.Where(e => e.StreamId == streamId)
                                     .ToListAsync(cancellationToken);

                return stream.OrderBy(e => e.Id)
                             .Select(EventSerializer.Desserialize);
            }
            catch
            { 
                return [];
            }
        }

        public void AppendToStream(string streamId, IEnumerable<IEvent> events)
        {
            var eventsToAppend = events.Select(e => EventSerializer.Serialize(EventIndex.Next(), streamId, e));
            Events.AddRange(eventsToAppend);
        }

        public long CurrentIndex() =>
            Events.Max(e => e.Id);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>().HasKey(e => e.Id);
            base.OnModelCreating(modelBuilder);
        }
    }

    internal record Event(long Id, string StreamId, string Created,
                          string Metadata, string EventType, string Data);
}
