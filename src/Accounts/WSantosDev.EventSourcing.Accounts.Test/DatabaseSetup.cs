using Microsoft.EntityFrameworkCore;

namespace WSantosDev.EventSourcing.Accounts.Test
{
    public class DatabaseSetup
    {
        public required AccountStore Store { get; init; }
        public required AccountReadModelStore ReadModelStore { get; init; }
        
        public required EventDbContext EventDbContext { get; init; }
        public required AccountReadModelDbContext ReadModelDbContext { get; init; }
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
                Store = new AccountStore(eventStore),
                ReadModelDbContext = readModelDbContext,
                ReadModelStore = new AccountReadModelStore(readModelDbContext)
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

            setup.ReadModelDbContext.Accounts.RemoveRange(setup.ReadModelDbContext.Accounts);
            setup.ReadModelDbContext.SaveChanges();
            setup.ReadModelDbContext.Dispose();
        }
    }
}
