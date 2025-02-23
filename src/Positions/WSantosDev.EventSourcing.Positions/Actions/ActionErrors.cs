using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Positions.Actions
{
    public static class ActionErrors
    {
        public static readonly PositionNotFoundError PositionNotFound;
    }

    public readonly struct PositionNotFoundError : IError;
}
