namespace WSantosDev.EventSourcing.Commons.Messaging
{
    public class InMemoryMessageBus : IMessageBus
    {
        private readonly Dictionary<Type, IList<IMessageHandler>> _subscribedHandlers = new();

        public void Subscribe(IMessageHandler messageHandler)
        {
            var messageType = GetEventType(messageHandler);
            if (_subscribedHandlers.TryGetValue(messageType, out var handlersList))
            {
                handlersList.Add(messageHandler);
                return;
            }

            handlersList = new List<IMessageHandler>() { messageHandler };
            _subscribedHandlers.Add(messageType, handlersList);
        }

        public void Publish<TMessage>(TMessage message) where TMessage : IMessage
        {
            if (!_subscribedHandlers.TryGetValue(typeof(TMessage), out var handlersList))
                return;

            for (var i = 0; i < handlersList.Count; i++)
                ((IMessageHandler<TMessage>)handlersList[i]).HandleAsync(message);
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
