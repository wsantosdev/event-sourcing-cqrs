using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Accounts
{
    public sealed partial class Account
    {
        private record struct AccountOpened(AccountId AccountId) : IEvent;
        private record struct AmountCredited(decimal Amount) : IEvent;
        private record struct AmountDebited(decimal Amount) : IEvent;
    }
}
