using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Positions.Commands
{
    public static class CommandErrors
    {
        public static readonly PositionNotFoundError PositionNotFound;
    }

    public readonly struct PositionNotFoundError : IError;
}
