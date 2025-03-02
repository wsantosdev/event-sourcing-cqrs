using Microsoft.EntityFrameworkCore;

namespace WSantosDev.EventSourcing.Positions.Test
{
    public class DatabaseSetup
    {
        public required PositionStore Store { get; init; }
        public required PositionReadModelStore ReadModelStore { get; init; }
        
        public required EventDbContext EventDbContext { get; init; }
        public required PositionReadModelDbContext ReadModelDbContext { get; init; }
    }

    public static class DatabaseSetupFactory
    {
        public static DatabaseSetup Create()
        {
            var options = SqliteDbContextOptionsBuilderExtensions.UseSqlite(new DbContextOptionsBuilder<EventDbContext>(), "Data Source=./Sqlite/EventStoreTest.sqlite").Options;
            var dbContext = new EventDbContext(options);
            var eventStore = new EventStore(dbContext);

            var readModelOptions = SqliteDbContextOptionsBuilderExtensions.UseSqlite(new DbContextOptionsBuilder<PositionReadModelDbContext>(), "Data Source=./Sqlite/ReadModelStoreTest.sqlite").Options;
            var readModelDbContext = new PositionReadModelDbContext(readModelOptions);

            return new DatabaseSetup
            {
                EventDbContext = dbContext,
                Store = new PositionStore(eventStore),
                ReadModelDbContext = readModelDbContext,
                ReadModelStore = new PositionReadModelStore(readModelDbContext)
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

            setup.ReadModelDbContext.Positions.RemoveRange(setup.ReadModelDbContext.Positions);
            setup.ReadModelDbContext.SaveChanges();
            setup.ReadModelDbContext.Dispose();
        }
    }
}
