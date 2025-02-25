using WSantosDev.EventSourcing.Positions;
using WSantosDev.EventSourcing.Positions.DomainEvents;
using WSantosDev.EventSourcing.Positions.Actions;
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
                           .AddTransient<DepositAction>()
                           .AddTransient<WithdrawAction>()
                           .AddSingleton<PositionsByAccountQuery>()
                           .AddSingleton<PositionBySymbolQuery>()
                           .AddSingleton<OrderPlacedHandler>()
                           .AddSingleton<ExchangeOrderExecutedHandler>()
                           .AddSingleton<PositionOpenedHandler>()
                           .AddSingleton<DepositedHandler>()
                           .AddSingleton<WithdrawnHandler>();
        }
    }
}
