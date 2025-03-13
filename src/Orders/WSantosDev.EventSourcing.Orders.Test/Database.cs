using Microsoft.EntityFrameworkCore;

namespace WSantosDev.EventSourcing.Orders.Test
{
    public class Database
    {
        public required OrderStore Store { get; init; }
        public required OrderViewStore ViewStore { get; init; }
        
        public required EventDbContext EventDbContext { get; init; }
        public required OrderViewDbContext ViewDbContext { get; init; }
    }

    public static class DatabaseFactory
    {
        public static Database Create()
        {
            var config = new SqliteConfig("Data Source=./Sqlite/EventStoreTest.sqlite");

            var options = SqliteDbContextOptionsBuilderExtensions.UseSqlite(new DbContextOptionsBuilder<EventDbContext>(), config.ConnectionString).Options;
            var dbContext = new EventDbContext(options);
            var eventStore = new EventStore(dbContext);

            var viewOptions = SqliteDbContextOptionsBuilderExtensions.UseSqlite(new DbContextOptionsBuilder<OrderViewDbContext>(), config.ConnectionString).Options;
            var viewDbContext = new OrderViewDbContext(viewOptions);
            var viewStore = new OrderViewStore(viewDbContext);

            return new Database
            {
                ViewDbContext = viewDbContext,
                ViewStore = viewStore,
                EventDbContext = dbContext,
                Store = new OrderStore(config),
            };
        }
    }

    public static class DatabaseDisposer
    {
        public static void Dispose(Database setup)
        {
            setup.EventDbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE Events");
            setup.EventDbContext.SaveChanges();
            setup.EventDbContext.Dispose();

            setup.ViewDbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE Accounts");
            setup.ViewDbContext.SaveChanges();
            setup.ViewDbContext.Dispose();
        }
    }
}
