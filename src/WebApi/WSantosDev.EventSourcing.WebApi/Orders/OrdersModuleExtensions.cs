using WSantosDev.EventSourcing.Orders;
using WSantosDev.EventSourcing.Orders.Actions;
using WSantosDev.EventSourcing.Orders.Queries;
using WSantosDev.EventSourcing.WebApi.Orders.DomainEvents;

namespace WSantosDev.EventSourcing.WebApi.Orders
{
    public static class OrdersModuleExtensions
    {
        public static IServiceCollection AddOrdersModule(this IServiceCollection services)
        {
            return 
            services.AddSingleton<OrderStore>()
                    .AddTransient<Place>()
                    .AddTransient<Execute>()
                    .AddTransient<OrdersByAccount>()
                    .AddSingleton<ExchangeOrderExecutedHandler>();
        }
    }
}
