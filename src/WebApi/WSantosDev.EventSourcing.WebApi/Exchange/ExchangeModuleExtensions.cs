using WSantosDev.EventSourcing.Exchange;
using WSantosDev.EventSourcing.Exchange.Commands;
using WSantosDev.EventSourcing.Exchange.DomainEvents;
using WSantosDev.EventSourcing.Exchange.Queries;
using WSantosDev.EventSourcing.WebApi.Exchange.DomainEvents;

namespace WSantosDev.EventSourcing.WebApi.Exchange
{
    public static class ExchangeModuleExtensions
    {
        public static IServiceCollection AddExchangeModule(this IServiceCollection services) 
        {
            return services.AddSingleton<IExchangeOrderStore, ExchangeOrderStore>()
                           .AddSingleton<IExchangeOrderReadModelStore, ExchangeOrderReadModelStore>()
                           .AddTransient<Create>()
                           .AddTransient<Execute>()
                           .AddTransient<AllExchangeOrders>()
                           .AddSingleton<OrderPlacedEventHandler>()
                           .AddSingleton<ExchangeOrderCreatedHandler>()
                           .AddSingleton<ExchangeOrderExecutedHandler>();
        }
    }
}
