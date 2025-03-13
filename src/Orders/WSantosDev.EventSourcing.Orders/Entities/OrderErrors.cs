using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Orders
{
    public static class Errors
    {
        public static readonly EmptyAccountIdError EmptyAccountId;
        public static readonly EmptyOrderIdError EmptyOrderId;
        public static readonly InvalidSideError InvalidSide;
        public static readonly InvalidQuantityError InvalidQuantity;
        public static readonly InvalidSymbolError InvalidSymbol;
        public static readonly InvalidPriceError InvalidPrice;

        public static readonly AlreadyFilledError AlreadyFilled;
    }


    public readonly struct EmptyAccountIdError : IError { }
    public readonly struct EmptyOrderIdError : IError { }
    public readonly struct InvalidSideError : IError { }
    public readonly struct InvalidQuantityError : IError { }
    public readonly struct InvalidSymbolError : IError { }
    public readonly struct InvalidPriceError : IError { }

    public readonly struct AlreadyFilledError : IError { }
}
