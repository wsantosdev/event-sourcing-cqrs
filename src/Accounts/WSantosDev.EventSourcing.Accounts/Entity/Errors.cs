using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Accounts
{
    public static class Errors
    {
        public static readonly EmptyAccountIdError EmptyAccountId;
        public static readonly InvalidAmountError InvalidAmount;
        public static readonly InsuficientFundsError InsufficientFunds;
    }

    public readonly struct EmptyAccountIdError : IError { }
    public readonly struct InvalidAmountError : IError { }
    public readonly struct InsuficientFundsError : IError { }
}
