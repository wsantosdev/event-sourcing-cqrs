using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Orders.Actions
{
    public static class ActionErrors
    {
        public static readonly OrderNotFoundError OrderNotFound;
    }

    public readonly struct OrderNotFoundError : IError;
}
