using Microsoft.EntityFrameworkCore;
using WSantosDev.EventSourcing.Accounts;
using WSantosDev.EventSourcing.EventStore;
using WSantosDev.EventSourcing.Exchange;
using WSantosDev.EventSourcing.Orders;
using WSantosDev.EventSourcing.Positions;

namespace WSantosDev.EventSourcing.WebApi
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection AddDatabases(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["ConnectionStrings:EventStore"]!;

            return services.AddSingleton(new SqliteConfig(connectionString))
                           .AddDbContext<EventDbContext>(options => options.UseSqlite(connectionString), ServiceLifetime.Singleton)
                           .AddDbContext<AccountViewDbContext>(options => 
                                        options.UseSqlite(configuration["ConnectionStrings:EventStore"]),
                                        ServiceLifetime.Scoped)
                           .AddDbContext<PositionViewDbContext>(options => 
                                        options.UseSqlite(configuration["ConnectionStrings:EventStore"]), 
                                        ServiceLifetime.Scoped)
                           .AddDbContext<ExchangeOrderViewDbContext>(options => 
                                        options.UseSqlite(configuration["ConnectionStrings:EventStore"]),
                                        ServiceLifetime.Scoped)
                           .AddDbContext<OrderViewDbContext>(options =>
                                        options.UseSqlite(configuration["ConnectionStrings:EventStore"]),
                                        ServiceLifetime.Scoped);
        }
    }
}
