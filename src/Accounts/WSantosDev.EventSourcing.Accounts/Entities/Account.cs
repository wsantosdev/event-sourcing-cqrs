using Moonad;
using WSantosDev.EventSourcing.Commons.Modeling;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Accounts.Entities;

namespace WSantosDev.EventSourcing.Accounts
{
    public sealed partial class Account : EventBasedEntity, ISnapshotable
    {
        private readonly IList<AccountEntry> _entries = [];

        public AccountId AccountId { get; private set; } = AccountId.Empty;
        
        public Money Balance => _entries.DefaultIfEmpty(AccountEntry.Empty)
                                        .Sum(e => e.Value);

        private Account(AccountId accountId) =>
            RaiseEvent(new AccountOpened(accountId));

        public Account(IEnumerable<IEvent> events) =>
            FeedEvents(events);

        public Account(ISnapshot snapshot)
        {
            var accountSnapshot = (snapshot as AccountSnapshot)!;
            
            AccountId = accountSnapshot.AccountId;
            foreach (AccountEntry entry in accountSnapshot.Entries)
                _entries.Add(entry);
            Version = accountSnapshot.Version;
        }

        public static Account Restore(ISnapshot snapshot, IEnumerable<IEvent> events)
        {
            var account = new Account(snapshot);
            account.FeedEvents(events);

            return account;
        }

        public static Result<Account, IError> Open(AccountId accountId, Money initialDeposit)
        {
            if (accountId == AccountId.Empty)
                return Errors.EmptyAccountId;

            var account = new Account(accountId);
            
            if (initialDeposit > Money.Zero)
                account.Credit(initialDeposit);

            return account;
        }

        private void Apply(AccountOpened @event) =>
            AccountId = @event.AccountId;

        public Result<IError> Credit(Money amount)
        {
            if (amount <= Money.Zero)
                return Result<IError>.Error(Errors.InvalidAmount);

            RaiseEvent(new AmountCredited(amount));

            return true;
        }

        private void Apply(AmountCredited @event) =>
            _entries.Add(AccountEntry.Credit(@event.Amount));

        public Result<IError> Debit(Money amount)
        {
            if(amount == Money.Zero)
                return Errors.InvalidAmount;
            if (Money.Zero > Balance - amount)
                return Errors.InsufficientFunds;

            RaiseEvent(new AmountDebited(amount));

            return true;
        }

        private void Apply(AmountDebited @event) =>
            _entries.Add(AccountEntry.Debit(@event.Amount));

        protected override void ProcessEvent(IEvent @event)
        {
            switch (@event) 
            {
                case AccountOpened accountOpened:
                    Apply(accountOpened); break;
                case AmountCredited amountCredited:
                    Apply(amountCredited); break;
                case AmountDebited amountDebited:
                    Apply(amountDebited); break;
            }
        }

        public bool ShouldTakeSnapshot() => _entries.Count % 3 == 0;

        public ISnapshot TakeSnapshot() =>
            new AccountSnapshot(AccountId, [.. _entries], Version);
    }
}
