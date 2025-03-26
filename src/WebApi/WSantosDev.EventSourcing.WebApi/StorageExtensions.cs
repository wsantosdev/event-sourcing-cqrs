using Microsoft.EntityFrameworkCore;
using WSantosDev.EventSourcing.SharedStorage;

namespace WSantosDev.EventSourcing.WebApi
{
    public static class StorageExtensions
    {
        public static IServiceCollection AddEventStore(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = $"Data Source={Path.Combine(Directory.GetCurrentDirectory(), configuration["Database:FileName"]!)}";

            return services.AddDbContext<EventDbContext>(options => options.UseSqlite(connectionString), ServiceLifetime.Singleton)
                           .AddDbContext<SnapshotDbContext>(options => options.UseSqlite(connectionString), ServiceLifetime.Singleton);
        }
    }
}
