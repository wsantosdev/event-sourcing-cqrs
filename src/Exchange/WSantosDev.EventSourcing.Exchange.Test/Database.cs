using Microsoft.EntityFrameworkCore;

namespace WSantosDev.EventSourcing.Exchange.Test
{
    public class Database
    {
        public required ExchangeOrderStore Store { get; init; }
        public required ExchangeOrderViewStore ViewStore { get; init; }
        
        public required EventDbContext EventDbContext { get; init; }
        public required ExchangeOrderViewDbContext ViewDbContext { get; init; }
    }

    public static class DatabaseFactory
    {
        public static Database Create()
        {
            var config = new SqliteConfig("Data Source=./Sqlite/ReadModelStoreTest.sqlite");

            var options = new DbContextOptionsBuilder().UseSqlite(config.ConnectionString).Options;
            var dbContext = new EventDbContext(new DbContextOptionsBuilder<EventDbContext>().UseSqlite(config.ConnectionString).Options);
            var eventStore = new EventStore(dbContext);

            var viewDbContext = new ExchangeOrderViewDbContext(new DbContextOptionsBuilder<ExchangeOrderViewDbContext>().UseSqlite(config.ConnectionString).Options);
            var viewStore = new ExchangeOrderViewStore(viewDbContext);

            return new Database
            {
                ViewDbContext = viewDbContext,
                ViewStore = viewStore,
                EventDbContext = dbContext,
                Store = new ExchangeOrderStore(config)
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
