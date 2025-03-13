using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Orders.Actions
{
    public static class CommandErrors
    {
        public static readonly OrderNotFoundError OrderNotFound;
    }

    public readonly struct OrderNotFoundError : IError;
}
