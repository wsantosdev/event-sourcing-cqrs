namespace WSantosDev.EventSourcing.Commons.Messaging
{
    public interface IMessageBus
    {
        void Publish<TEvent>(TEvent @event) where TEvent : IMessage;
        void Subscribe(IMessageHandler eventHandler);
    }
}
