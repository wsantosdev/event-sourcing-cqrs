using Microsoft.EntityFrameworkCore;

namespace WSantosDev.EventSourcing.Positions.Test
{
    public class Database
    {
        public required PositionStore Store { get; init; }
        public required PositionViewStore ViewStore { get; init; }
        
        public required EventDbContext EventDbContext { get; init; }
        public required PositionViewDbContext ViewDbContext { get; init; }
    }

    public static class DatabaseSetupFactory
    {
        public static Database Create()
        {
            var config = new SqliteConfig("Data Source=./Sqlite/EventStoreTest.sqlite");

            var options = SqliteDbContextOptionsBuilderExtensions.UseSqlite(new DbContextOptionsBuilder<EventDbContext>(), config.ConnectionString).Options;
            var dbContext = new EventDbContext(options);
            var eventStore = new EventStore(dbContext);

            var viewOptions = SqliteDbContextOptionsBuilderExtensions.UseSqlite(new DbContextOptionsBuilder<PositionViewDbContext>(), config.ConnectionString).Options;
            var viewDbContext = new PositionViewDbContext(viewOptions);
            var readModelStore = new PositionViewStore(viewDbContext);

            return new Database
            {
                ViewDbContext = viewDbContext,
                ViewStore = readModelStore,
                EventDbContext = dbContext,
                Store = new PositionStore(config)
            };
        }
    }

    public static class DatabaseSetupDisposer
    {
        public static void Dispose(Database setup)
        {
            setup.EventDbContext.Database.ExecuteSqlRaw("DELETE FROM Events");
            setup.EventDbContext.SaveChanges();
            setup.EventDbContext.Dispose();

            setup.ViewDbContext.Database.ExecuteSqlRaw("DELETE FROM ExchangeOrders");
            setup.ViewDbContext.SaveChanges();
            setup.ViewDbContext.Dispose();
        }
    }
}
