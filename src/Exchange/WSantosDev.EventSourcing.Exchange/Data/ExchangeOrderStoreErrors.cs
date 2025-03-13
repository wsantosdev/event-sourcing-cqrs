using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Exchange
{
    public static class ExchangeOrderStoreErrors
    {
        public static readonly StorageUnavailableError StorageUnavailable;
    }

    public readonly struct StorageUnavailableError : IError;
}
