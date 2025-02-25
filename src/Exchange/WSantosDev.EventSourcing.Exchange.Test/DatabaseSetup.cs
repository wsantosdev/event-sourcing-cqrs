using Microsoft.EntityFrameworkCore;
using WSantosDev.EventSourcing.WebApi.Exchange;

namespace WSantosDev.EventSourcing.Exchange.Test
{
    public class DatabaseSetup
    {
        public required ExchangeOrderStore Store { get; init; }
        public required ExchangeOrderReadModelStore ReadModelStore { get; init; }
        
        public required EventDbContext EventDbContext { get; init; }
        public required ExchangeOrderReadModelDbContext ReadModelDbContext { get; init; }
    }

    public static class DatabaseSetupFactory
    {
        public static DatabaseSetup Create()
        {
            var options = SqliteDbContextOptionsBuilderExtensions.UseSqlite(new DbContextOptionsBuilder<EventDbContext>(), "Data Source=./Sqlite/EventStoreTest.sqlite").Options;
            var dbContext = new EventDbContext(options);
            var eventStore = new EventStore(dbContext);

            var readModelOptions = SqliteDbContextOptionsBuilderExtensions.UseSqlite(new DbContextOptionsBuilder<ExchangeOrderReadModelDbContext>(), "Data Source=./Sqlite/ReadModelStoreTest.sqlite").Options;
            var readModelDbContext = new ExchangeOrderReadModelDbContext(readModelOptions);

            return new DatabaseSetup
            {
                EventDbContext = dbContext,
                Store = new ExchangeOrderStore(eventStore),
                ReadModelDbContext = readModelDbContext,
                ReadModelStore = new ExchangeOrderReadModelStore(readModelDbContext)
            };
        }
    }

    public static class DatabaseSetupDisposer
    {
        public static void Dispose(DatabaseSetup setup)
        {
            setup.EventDbContext.Events.RemoveRange(setup.EventDbContext.Events);
            setup.EventDbContext.SaveChanges();
            setup.EventDbContext.Dispose();

            setup.ReadModelDbContext.ExchangeOrders.RemoveRange(setup.ReadModelDbContext.ExchangeOrders);
            setup.ReadModelDbContext.SaveChanges();
            setup.ReadModelDbContext.Dispose();
        }
    }
}
