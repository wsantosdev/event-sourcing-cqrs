using WSantosDev.EventSourcing.Positions;
using WSantosDev.EventSourcing.Positions.Commands;
using WSantosDev.EventSourcing.Positions.Queries;
using WSantosDev.EventSourcing.WebApi.Positions.DomainEvents;

namespace WSantosDev.EventSourcing.WebApi.Positions
{
    public static class PositionModuleExtensions
    {
        public static IServiceCollection AddPositionsModule(this IServiceCollection services)
        {
            return services.AddSingleton<PositionStore>()
                           .AddTransient<Deposit>()
                           .AddTransient<Withdraw>()
                           .AddTransient<PositionsByAccount>()
                           .AddTransient<PositionBySymbol>()
                           .AddSingleton<OrderPlacedHandler>()
                           .AddSingleton<OrderExecutedHandler>();
        }
    }
}
