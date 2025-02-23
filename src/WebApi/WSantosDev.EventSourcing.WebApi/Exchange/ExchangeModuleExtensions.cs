using WSantosDev.EventSourcing.Exchange;
using WSantosDev.EventSourcing.Exchange.Actions;
using WSantosDev.EventSourcing.Exchange.ExternalEvents;
using WSantosDev.EventSourcing.Exchange.Queries;
using WSantosDev.EventSourcing.WebApi.Exchange.ExternalEvents;

namespace WSantosDev.EventSourcing.WebApi.Exchange
{
    public static class ExchangeModuleExtensions
    {
        public static IServiceCollection AddExchangeModule(this IServiceCollection services) 
        {
            return services.AddSingleton<IOrderStore, ExchangeOrderStore>()
                           .AddSingleton<IOrderReadModelStore, ExchangeOrderReadModelStore>()
                           .AddTransient<CreateAction>()
                           .AddTransient<ExecuteAction>()
                           .AddTransient<ExchangeOrdersQuery>()
                           .AddSingleton<OrderPlacedEventHandler>()
                           .AddSingleton<ExchangeCreatedHandler>()
                           .AddSingleton<ExchangeExecutedHandler>();
        }
    }
}
