using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
            EventIndex.Seed(_context.CurrentIndex());
        }

        public async Task<IEnumerable<IEvent>> ReadAsync(string streamId) =>
            await _context.ReadStreamAsync(streamId);

        public async Task<Result<IError>> AppendAsync(string streamId, IEnumerable<IEvent> events)
        {
            var toAppend = events.Select(@event => EventSerializer.Serialize(EventIndex.Next(), streamId, @event));

            _context.AddRange(toAppend);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync() =>
            await _context.Database.BeginTransactionAsync();
    }
}
