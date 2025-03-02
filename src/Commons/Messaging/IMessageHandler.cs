namespace WSantosDev.EventSourcing.Commons.Messaging
{
    public interface IMessageHandler { }

    public interface IMessageHandler<TMessage> : IMessageHandler where TMessage : IMessage
    {
        Task HandleAsync(TMessage message);
    }
}