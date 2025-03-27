using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Accounts.Commands
{
    public static class CommandErrors
    {
        public static readonly AccountAlreadyOpenError AccountAlreadyOpen;
        public static readonly AccountNotFoundError AccountNotFound;
    }

    public readonly struct AccountAlreadyOpenError : IError;
    public readonly struct AccountNotFoundError : IError;
}
