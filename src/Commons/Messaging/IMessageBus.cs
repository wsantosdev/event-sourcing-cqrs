namespace WSantosDev.EventSourcing.Commons.Messaging
{
    public interface IMessageBus
    {
        void Publish<TMessage>(TMessage @event) where TMessage : IMessage;
        void Subscribe(IMessageHandler eventHandler);
    }
}
