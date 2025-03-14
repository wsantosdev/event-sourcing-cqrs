using Microsoft.EntityFrameworkCore;
using WSantosDev.EventSourcing.EventStore;

namespace WSantosDev.EventSourcing.Accounts.Test
{
    public class Database
    {
        public required AccountStore Store { get; init; }
        public required AccountViewStore ViewStore { get; init; }
        
        public required EventDbContext EventDbContext { get; init; }
        public required AccountViewDbContext ViewDbContext { get; init; }
    }

    public static class DatabaseFactory
    {
        public static Database Create()
        {
            var config = new SqliteConfig("Data Source=./Sqlite/EventSourcing.sqlite");
            var eventDbContext = new EventDbContext(new DbContextOptionsBuilder<EventDbContext>().UseSqlite(config.ConnectionString).Options);
            var viewDbContext = new AccountViewDbContext(new DbContextOptionsBuilder<AccountViewDbContext>().UseSqlite(config.ConnectionString).Options);
            var viewStore = new AccountViewStore(viewDbContext);

            return new Database
            {
                ViewDbContext = viewDbContext,
                ViewStore = viewStore,
                EventDbContext = eventDbContext,
                Store = new AccountStore(config),
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
