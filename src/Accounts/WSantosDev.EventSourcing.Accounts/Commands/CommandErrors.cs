using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Accounts.Commands
{
    public static class CommandErrors
    {
        public static readonly AccountAlreadyOpenedError AccountAlreadyOpened;
        public static readonly AccountNotFoundError AccountNotFound;
    }

    public readonly struct AccountAlreadyOpenedError : IError;
    public readonly struct AccountNotFoundError : IError;
}
