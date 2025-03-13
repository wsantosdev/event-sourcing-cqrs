using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Positions
{
    public static class PositionStoreErrors
    {
        public static readonly StorageUnavailableError StorageUnavailable;
    }

    public readonly struct StorageUnavailableError : IError;
}
