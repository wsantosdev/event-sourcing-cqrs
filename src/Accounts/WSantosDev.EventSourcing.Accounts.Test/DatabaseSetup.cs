using Microsoft.EntityFrameworkCore;

namespace WSantosDev.EventSourcing.Accounts.Test
{
    public class DatabaseSetup
    {
        public required AccountStore AccountStore { get; init; }
        public required AccountReadModelStore AccountReadModelStore { get; init; }
        
        public required EventDbContext EventDbContext { get; init; }
        public required AccountReadModelDbContext AccountReadModelDbContext { get; init; }
    }

    public static class DatabaseSetupFactory
    {
        public static DatabaseSetup Create()
        {
            var options = SqliteDbContextOptionsBuilderExtensions.UseSqlite(new DbContextOptionsBuilder<EventDbContext>(), "Data Source=./Sqlite/EventStoreTest.sqlite").Options;
            var dbContext = new EventDbContext(options);
            var eventStore = new EventStore(dbContext);

            var readModelOptions = SqliteDbContextOptionsBuilderExtensions.UseSqlite(new DbContextOptionsBuilder<AccountReadModelDbContext>(), "Data Source=./Sqlite/ReadModelStoreTest.sqlite").Options;
            var readModelDbContext = new AccountReadModelDbContext(readModelOptions);

            return new DatabaseSetup
            {
                EventDbContext = dbContext,
                AccountStore = new AccountStore(eventStore),
                AccountReadModelDbContext = readModelDbContext,
                AccountReadModelStore = new AccountReadModelStore(readModelDbContext)
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

            setup.AccountReadModelDbContext.Accounts.RemoveRange(setup.AccountReadModelDbContext.Accounts);
            setup.AccountReadModelDbContext.SaveChanges();
            setup.AccountReadModelDbContext.Dispose();
        }
    }
}
