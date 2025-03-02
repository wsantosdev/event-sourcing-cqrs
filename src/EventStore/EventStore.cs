using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Moonad;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing
{
    public class EventStore
    {
        private readonly EventDbContext _context;
        
        public EventStore(EventDbContext context)
        { 
            _context = context;
            EventIndex.Seed(_context.Events.Count());
        }

        public async Task<Result<IError>> AppendAsync(string streamId, IEnumerable<IEvent> events)
        {
            try
            {
                var toAppend = events.Select(@event => EventSerializer.Serialize(EventIndex.Next(),
                                                                              streamId,
                                                                              @event));

                _context.Events.AddRange(toAppend);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return EventStoreErrors.CouldNotPersistEvents;
            }
        }

        public async Task<IEnumerable<IEvent>> ReadAsync(string streamId) =>
        
            (await _context.Events
                           .Where(e => e.StreamId == streamId)
                           .ToListAsync())
                           .OrderBy(e => e.Id)
                           .Select(EventSerializer.Desserialize);
    }

    public static class EventStoreErrors
    {
        public static readonly CouldNotPersistEventsError CouldNotPersistEvents;
    }

    public readonly struct CouldNotPersistEventsError : IError;

    public sealed class EventDbContext(DbContextOptions<EventDbContext> options) : DbContext(options)
    {
        public DbSet<Event> Events { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>().HasKey(e => e.Id);
            base.OnModelCreating(modelBuilder);
        }
    }

    internal sealed class EventSerializer
    {
        public static Event Serialize(long eventId, string streamId, IEvent @event)
        {
            return new(eventId,
                       streamId,
                       DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                       string.Empty,
                       @event.GetType().FullName!,
                       JsonSerializer.Serialize(@event, @event.GetType()));
        }
        
        public static IEvent Desserialize(Event @event) =>
            (IEvent)JsonSerializer.Deserialize(@event.Data, GetType(@event.EventType))!;

        private static Type GetType(string typeName)
        {
            return AppDomain.CurrentDomain
                            .GetAssemblies()
                            .SelectMany(a => a.GetTypes().Where(x => x.FullName == typeName))
                            .First();
        }
    }

    public static class EventIndex
    {
        private static long _index = 0;
        
        public static long Next() =>
            Interlocked.Increment(ref _index);

        public static void Seed(long value) =>
            Interlocked.Exchange(ref _index, value);
    }

    public record Event(long Id, string StreamId, string Created, 
                        string Metadata, string EventType, string Data);
}
