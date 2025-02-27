using Microsoft.EntityFrameworkCore;

namespace WSantosDev.EventSourcing.Orders.Test
{
    public class DatabaseSetup
    {
        public required OrderStore Store { get; init; }
        public required OrderReadModelStore ReadModelStore { get; init; }
        
        public required EventDbContext EventDbContext { get; init; }
        public required OrderReadModelDbContext ReadModelDbContext { get; init; }
    }

    public static class DatabaseSetupFactory
    {
        public static DatabaseSetup Create()
        {
            var options = SqliteDbContextOptionsBuilderExtensions.UseSqlite(new DbContextOptionsBuilder<EventDbContext>(), "Data Source=./Sqlite/EventStoreTest.sqlite").Options;
            var dbContext = new EventDbContext(options);
            var eventStore = new EventStore(dbContext);

            var readModelOptions = SqliteDbContextOptionsBuilderExtensions.UseSqlite(new DbContextOptionsBuilder<OrderReadModelDbContext>(), "Data Source=./Sqlite/ReadModelStoreTest.sqlite").Options;
            var readModelDbContext = new OrderReadModelDbContext(readModelOptions);

            return new DatabaseSetup
            {
                EventDbContext = dbContext,
                Store = new OrderStore(eventStore),
                ReadModelDbContext = readModelDbContext,
                ReadModelStore = new OrderReadModelStore(readModelDbContext)
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

            setup.ReadModelDbContext.Orders.RemoveRange(setup.ReadModelDbContext.Orders);
            setup.ReadModelDbContext.SaveChanges();
            setup.ReadModelDbContext.Dispose();
        }
    }
}
