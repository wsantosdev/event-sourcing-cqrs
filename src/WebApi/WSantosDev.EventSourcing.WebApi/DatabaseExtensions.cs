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

            return services.AddDbContext<EventDbContext>(options => options.UseSqlite(connectionString), ServiceLifetime.Singleton)
                           .AddDbContext<AccountViewDbContext>(options => 
                                        options.UseSqlite(connectionString),
                                        ServiceLifetime.Singleton)
                           .AddDbContext<PositionViewDbContext>(options => 
                                        options.UseSqlite(connectionString), 
                                        ServiceLifetime.Singleton)
                           .AddDbContext<ExchangeOrderViewDbContext>(options => 
                                        options.UseSqlite(connectionString),
                                        ServiceLifetime.Singleton)
                           .AddDbContext<OrderViewDbContext>(options =>
                                        options.UseSqlite(connectionString),
                                        ServiceLifetime.Singleton);
        }
    }
}
