using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Exchange
{
    public static class Errors
    {
        public static readonly AlreadyCanceledError AlreadyCanceled;
        public static readonly AlreadyFilledError AlreadyFilled;
    }

    public readonly struct AlreadyCanceledError : IError { }
    public readonly struct AlreadyFilledError : IError { }
}
