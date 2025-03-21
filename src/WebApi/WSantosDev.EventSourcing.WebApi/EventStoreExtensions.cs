using Microsoft.EntityFrameworkCore;
using WSantosDev.EventSourcing.EventStore;

namespace WSantosDev.EventSourcing.WebApi
{
    public static class EventStoreExtensions
    {
        public static IServiceCollection AddEventStore(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["ConnectionStrings:EventStore"]!;
            
            return services.AddDbContext<EventDbContext>(options => options.UseSqlite(connectionString), ServiceLifetime.Singleton)
                           .AddDbContext<SnapshotDbContext>(options => options.UseSqlite(connectionString), ServiceLifetime.Singleton);
        }
    }
}
