using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.WebApi
{
    public static class MessageBusExtensions
    {
        public static IServiceCollection AddMessageBus(this IServiceCollection services) 
        {
            return services.AddSingleton<IMessageBus, InMemoryMessageBus>();
        }

        public static IApplicationBuilder UseMessageBus(this IApplicationBuilder builder)
        {
            using var scope = builder.ApplicationServices.CreateScope();
            var messageBus = scope.ServiceProvider.GetRequiredService<IMessageBus>();

            var handlerTypes = GetHandlerTypes();
            foreach (var handlerType in handlerTypes)
            {
                var handler = scope.ServiceProvider.GetRequiredService(handlerType);
                messageBus.Subscribe((IMessageHandler)handler);
            }
            
            return builder;
        }

        private static Type[] GetHandlerTypes()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var handlers = assemblies.SelectMany(a => 
                                                a.DefinedTypes.Where(type => 
                                                    type.ImplementedInterfaces.Any(i => 
                                                        i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMessageHandler<>))))
                                     
                                     .ToArray();

            return handlers;
        }
    }
}
