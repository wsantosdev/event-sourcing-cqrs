namespace WSantosDev.EventSourcing.Commons.Messaging
{
    public class InMemoryMessageBus : IMessageBus
    {
        private readonly Dictionary<Type, IList<IMessageHandler>> _subscribedHandlers = new();

        public void Subscribe(IMessageHandler eventHandler)
        {
            var eventType = GetEventType(eventHandler);
            if (_subscribedHandlers.TryGetValue(eventType, out var handlersList))
            {
                handlersList.Add(eventHandler);
                return;
            }

            handlersList = new List<IMessageHandler>() { eventHandler };
            _subscribedHandlers.Add(eventType, handlersList);
        }

        public void Publish<TEvent>(TEvent @event) where TEvent : IMessage
        {
            if (!_subscribedHandlers.TryGetValue(typeof(TEvent), out var handlersList))
                return;

            for (var i = 0; i < handlersList.Count; i++)
                ((IMessageHandler<TEvent>)handlersList[i]).Handle(@event);
        }

        private static Type GetEventType(object eventHandler)
        {
            return eventHandler.GetType()
                               .GetInterfaces()
                               .First(i => i.GetGenericTypeDefinition() == typeof(IMessageHandler<>))
                               .GenericTypeArguments[0];
        }
    }
}
