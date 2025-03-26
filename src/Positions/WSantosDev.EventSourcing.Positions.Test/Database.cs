using Microsoft.EntityFrameworkCore;
using WSantosDev.EventSourcing.SharedStorage;

namespace WSantosDev.EventSourcing.Positions.Test
{
    public class Database
    {
        public required PositionStore Store { get; init; }
        public required PositionViewStore ViewStore { get; init; }
        
        public required EventDbContext EventDbContext { get; init; }
        public required PositionViewDbContext ViewDbContext { get; init; }
    }

    public static class DatabaseFactory
    {
        public static Database Create()
        {
            var connectionString = "Data Source=./Sqlite/EventSourcing.sqlite";

            var options = SqliteDbContextOptionsBuilderExtensions.UseSqlite(new DbContextOptionsBuilder<EventDbContext>(), connectionString).Options;
            var eventDbContext = new EventDbContext(options);
            
            var viewOptions = SqliteDbContextOptionsBuilderExtensions.UseSqlite(new DbContextOptionsBuilder<PositionViewDbContext>(), connectionString).Options;
            var viewDbContext = new PositionViewDbContext(viewOptions);
            var readModelStore = new PositionViewStore(viewDbContext);

            return new Database
            {
                ViewDbContext = viewDbContext,
                ViewStore = readModelStore,
                EventDbContext = eventDbContext,
                Store = new PositionStore(eventDbContext, viewDbContext)
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

            setup.ViewDbContext.Database.ExecuteSqlRaw("DELETE FROM ExchangeOrders");
            setup.ViewDbContext.SaveChanges();
            setup.ViewDbContext.Dispose();
        }
    }
}
