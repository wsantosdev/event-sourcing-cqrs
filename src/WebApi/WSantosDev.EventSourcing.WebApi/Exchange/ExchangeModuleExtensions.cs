using System.Reflection;
using Microsoft.EntityFrameworkCore;
using WSantosDev.EventSourcing.Exchange;
using WSantosDev.EventSourcing.Exchange.Commands;
using WSantosDev.EventSourcing.Exchange.Queries;
using WSantosDev.EventSourcing.WebApi.Exchange.DomainEvents;

namespace WSantosDev.EventSourcing.WebApi.Exchange
{
    public static class ExchangeModuleExtensions
    {
        public static IServiceCollection AddExchangeModule(this IServiceCollection services, IConfiguration configuration) 
        {
            var databaseFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
            var databaseFile = configuration["Database:FileName"]!;
            var connectionString = $"Data Source={Path.Combine(databaseFolder, databaseFile)}";

            return services.AddDbContext<ExchangeOrderViewDbContext>(options => options.UseSqlite(connectionString), ServiceLifetime.Singleton)
                           .AddSingleton<ExchangeOrderStore>()
                           .AddTransient<Create>()
                           .AddTransient<Execute>()
                           .AddTransient<ListExchangeOrders>()
                           .AddSingleton<OrderPlacedHandler>();
        }
    }
}
