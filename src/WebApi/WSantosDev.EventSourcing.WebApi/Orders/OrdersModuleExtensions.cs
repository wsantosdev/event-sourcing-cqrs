using WSantosDev.EventSourcing.Orders;
using WSantosDev.EventSourcing.Orders.Actions;
using WSantosDev.EventSourcing.Orders.ExternalEvents;
using WSantosDev.EventSourcing.Orders.Queries;
using WSantosDev.EventSourcing.WebApi.Orders.Actions;
using WSantosDev.EventSourcing.WebApi.Orders.ExternalEvents;

namespace WSantosDev.EventSourcing.WebApi.Orders
{
    public static class OrdersModuleExtensions
    {
        public static IServiceCollection AddOrdersModule(this IServiceCollection services)
        {
            return 
            services.AddSingleton<IOrderStore, OrderStore>()
                    .AddSingleton<IOrderReadModelStore, OrderReadModelStore>()
                    .AddTransient<PlaceAction>()
                    .AddTransient<ExecuteAction>()
                    .AddTransient<OrdersByAccountQuery>()
                    .AddSingleton<OrderPlacedHandler>()
                    .AddSingleton<OrderExecutedHandler>()
                    .AddSingleton<ExchangeExecutedHandler>();
        }
    }
}
