using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Orders
{
    public static class OrderStoreErrors
    {
        public static readonly StorageUnavailableError StorageUnavailable;
    }

    public readonly struct StorageUnavailableError : IError;
}
