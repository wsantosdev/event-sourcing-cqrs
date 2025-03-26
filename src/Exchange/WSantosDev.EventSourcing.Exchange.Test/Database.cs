using Microsoft.EntityFrameworkCore;
using WSantosDev.EventSourcing.SharedStorage;

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
            var connectionString = "Data Source=./Sqlite/EventSourcing.sqlite";

            var options = SqliteDbContextOptionsBuilderExtensions.UseSqlite(new DbContextOptionsBuilder<EventDbContext>(), connectionString).Options;
            var eventDbContext = new EventDbContext(options);
            
            var viewOptions = SqliteDbContextOptionsBuilderExtensions.UseSqlite(new DbContextOptionsBuilder<ExchangeOrderViewDbContext>(), connectionString).Options;
            var viewDbContext = new ExchangeOrderViewDbContext(viewOptions);
            var viewStore = new ExchangeOrderViewStore(viewDbContext);

            return new Database
            {
                ViewDbContext = viewDbContext,
                ViewStore = viewStore,
                EventDbContext = eventDbContext,
                Store = new ExchangeOrderStore(eventDbContext, viewDbContext)
            };
        }
    }

    public static class DatabaseDisposer
    {
        public static void Dispose(Database setup)
        {
            setup.EventDbContext.Database.ExecuteSqlRaw("DELETE FROM Events");
            setup.EventDbContext.Database.ExecuteSqlRaw("DELETE FROM ExchangeOrders");
            setup.EventDbContext.SaveChanges();
            setup.EventDbContext.Dispose();
        }
    }
}
