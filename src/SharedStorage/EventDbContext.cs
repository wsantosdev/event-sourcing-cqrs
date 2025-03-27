using Microsoft.EntityFrameworkCore;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.SharedStorage
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

        public async Task<IEnumerable<IEvent>> ReadStreamFromEventIdAsync(string streamId, long eventId, CancellationToken cancellationToken = default)
        {
            try
            {
                var stream = await Events.Where(e => e.StreamId == streamId && e.Id > eventId)
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
            var eventsToAppend = events.Select(e => EventSerializer.Serialize(e.Id, streamId, e));
            Events.AddRange(eventsToAppend);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Event>().HasKey(e => new { e.Id, e.StreamId });
            base.OnModelCreating(modelBuilder);
        }
    }

    internal record Event(int Id, string StreamId, string Created, string EventType, string Data);
}
