using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Accounts.Commands
{
    public static class CommandErrors
    {
        public static readonly AccountAlreadyExistsError AccountAlreadyExists;
        public static readonly AccountNotFoundError AccountNotFound;
    }

    public readonly struct AccountAlreadyExistsError : IError;
    public readonly struct AccountNotFoundError : IError;
}
