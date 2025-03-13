using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Exchange.Commands
{
    public static class CommandsErrors
    {
        public static readonly OrderNotFoundError OrderNotFound;
    }

    public readonly struct OrderNotFoundError : IError;
}
