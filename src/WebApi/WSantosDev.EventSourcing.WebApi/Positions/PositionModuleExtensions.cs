using System.Reflection;
using Microsoft.EntityFrameworkCore;
using WSantosDev.EventSourcing.Positions;
using WSantosDev.EventSourcing.Positions.Commands;
using WSantosDev.EventSourcing.Positions.Queries;
using WSantosDev.EventSourcing.WebApi.Positions.DomainEvents;

namespace WSantosDev.EventSourcing.WebApi.Positions
{
    public static class PositionModuleExtensions
    {
        public static IServiceCollection AddPositionsModule(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
            var databaseFile = configuration["Database:FileName"]!;
            var connectionString = $"Data Source={Path.Combine(databaseFolder, databaseFile)}";

            return services.AddDbContext<PositionViewDbContext>(options => options.UseSqlite(connectionString), ServiceLifetime.Singleton)
                           .AddSingleton<PositionStore>()
                           .AddTransient<Deposit>()
                           .AddTransient<Withdraw>()
                           .AddTransient<PositionsByAccount>()
                           .AddTransient<PositionBySymbol>()
                           .AddSingleton<OrderPlacedHandler>()
                           .AddSingleton<OrderExecutedHandler>();
        }
    }
}
