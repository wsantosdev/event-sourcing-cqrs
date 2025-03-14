using Microsoft.EntityFrameworkCore;
using WSantosDev.EventSourcing.EventStore;

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
            var config = new SqliteConfig("Data Source=./Sqlite/EventSourcing.sqlite");

            var options = SqliteDbContextOptionsBuilderExtensions.UseSqlite(new DbContextOptionsBuilder<EventDbContext>(), config.ConnectionString).Options;
            var dbContext = new EventDbContext(options);
            
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
            setup.EventDbContext.Database.ExecuteSqlRaw("DELETE FROM Events");
            setup.EventDbContext.SaveChanges();
            setup.EventDbContext.Dispose();

            setup.ViewDbContext.Database.ExecuteSqlRaw("DELETE FROM Accounts");
            setup.ViewDbContext.SaveChanges();
            setup.ViewDbContext.Dispose();
        }
    }
}
