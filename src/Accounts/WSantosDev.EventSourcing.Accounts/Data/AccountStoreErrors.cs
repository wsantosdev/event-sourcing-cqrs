using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Accounts
{
    public static class AccountStoreErrors
    {
        public static readonly StorageUnavailableError StorageUnavailable;
    }

    public readonly struct StorageUnavailableError : IError;
}
