using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Accounts.Actions
{
    public static class ActionErrors
    {
        public static readonly AccountAlreadyExistsError AccountAlreadyExists;
        public static readonly AccountNotFoundError AccountNotFound;
    }

    public readonly struct AccountAlreadyExistsError : IError;
    public readonly struct AccountNotFoundError : IError;
}
