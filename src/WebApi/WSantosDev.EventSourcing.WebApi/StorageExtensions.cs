using System.Reflection;
using Microsoft.EntityFrameworkCore;
using WSantosDev.EventSourcing.SharedStorage;

namespace WSantosDev.EventSourcing.WebApi
{
    public static class StorageExtensions
    {
        public static IServiceCollection AddEventStore(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
            var databaseFile = configuration["Database:FileName"]!;
            var connectionString = $"Data Source={Path.Combine(databaseFolder, databaseFile)}";

            return services.AddDbContext<EventDbContext>(options => options.UseSqlite(connectionString), ServiceLifetime.Singleton)
                           .AddDbContext<SnapshotDbContext>(options => options.UseSqlite(connectionString), ServiceLifetime.Singleton);
        }
    }
}
