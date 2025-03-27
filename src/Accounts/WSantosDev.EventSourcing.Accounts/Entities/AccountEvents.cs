using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Accounts
{
    public sealed partial class Account
    {
        private record struct AccountOpened(int Id, AccountId AccountId) : IEvent;
        private record struct AmountCredited(int Id, decimal Amount) : IEvent;
        private record struct AmountDebited(int Id, decimal Amount) : IEvent;
    }
}
