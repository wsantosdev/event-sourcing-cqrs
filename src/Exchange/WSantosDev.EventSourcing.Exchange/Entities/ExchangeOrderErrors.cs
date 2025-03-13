using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Exchange
{
    public static class Errors
    {
        public static readonly AlreadyFilledError AlreadyFilled;
    }

    public readonly struct AlreadyFilledError : IError { }
}
