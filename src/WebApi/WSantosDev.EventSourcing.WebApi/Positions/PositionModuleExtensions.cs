using WSantosDev.EventSourcing.Positions;
using WSantosDev.EventSourcing.Positions.DomainEvents;
using WSantosDev.EventSourcing.Positions.Commands;
using WSantosDev.EventSourcing.WebApi.Positions.DomainEvents;
using WSantosDev.EventSourcing.Positions.Queries;

namespace WSantosDev.EventSourcing.WebApi.Positions
{
    public static class PositionModuleExtensions
    {
        public static IServiceCollection AddPositionsModule(this IServiceCollection services)
        {
            return services.AddSingleton<IPositionStore, PositionStore>()
                           .AddSingleton<IPositionReadModelStore, PositionReadModelStore>()
                           .AddTransient<Deposit>()
                           .AddTransient<Withdraw>()
                           .AddSingleton<PositionsByAccount>()
                           .AddSingleton<PositionBySymbol>()
                           .AddSingleton<OrderPlacedHandler>()
                           .AddSingleton<ExchangeOrderExecutedHandler>()
                           .AddSingleton<PositionOpenedHandler>()
                           .AddSingleton<DepositedHandler>()
                           .AddSingleton<WithdrawnHandler>();
        }
    }
}
