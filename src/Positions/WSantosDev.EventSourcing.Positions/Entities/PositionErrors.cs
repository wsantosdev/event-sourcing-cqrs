using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Positions
{
    public static class Errors
    {
        public static readonly InvalidAccountIdError EmptyAccountId;
        public static readonly EmptySymbolError EmptySymbol;
        public static readonly QuantityZeroError QuantityZero;
        public static readonly InsuficientSharesError InsuficientShares;
    }

    public readonly struct InvalidAccountIdError : IError { }
    public readonly struct EmptySymbolError : IError { }
    public readonly struct QuantityZeroError : IError { }
    public readonly struct InsuficientSharesError : IError { }
}
