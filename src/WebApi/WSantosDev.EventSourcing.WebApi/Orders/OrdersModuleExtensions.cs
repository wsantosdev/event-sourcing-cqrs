using Microsoft.EntityFrameworkCore;
using WSantosDev.EventSourcing.Orders;
using WSantosDev.EventSourcing.Orders.Commands;
using WSantosDev.EventSourcing.Orders.Queries;
using WSantosDev.EventSourcing.WebApi.Orders.DomainEvents;

namespace WSantosDev.EventSourcing.WebApi.Orders
{
    public static class OrdersModuleExtensions
    {
        public static IServiceCollection AddOrdersModule(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["ConnectionStrings:EventStore"]!;

            return services.AddDbContext<OrderViewDbContext>(options => options.UseSqlite(connectionString), ServiceLifetime.Singleton)
                           .AddSingleton<OrderStore>()
                           .AddTransient<Place>()
                           .AddTransient<Execute>()
                           .AddTransient<OrdersByAccount>()
                           .AddSingleton<ExchangeOrderExecutedHandler>();
        }
    }
}
