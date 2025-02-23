namespace WSantosDev.EventSourcing.Commons.Messaging
{
    public interface IMessageHandler { }

    public interface IMessageHandler<TMessage> : IMessageHandler where TMessage : IMessage
    {
        void Handle(TMessage @event);
    }
}