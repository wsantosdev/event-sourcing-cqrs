﻿using Moonad;
using WSantosDev.EventSourcing.Commons.Modeling;
using WSantosDev.EventSourcing.Commons;

namespace WSantosDev.EventSourcing.Accounts
{
    public sealed partial class Account : EventBasedEntity, ISnapshotable<AccountSnapshot>
    {
        private readonly IList<Entry> _entries = [];

        public AccountId AccountId { get; private set; } = AccountId.Empty;
        
        public Money Balance => _entries.DefaultIfEmpty(Entry.Empty)
                                        .Sum(e => e.Value);

        private Account(AccountId accountId) =>
            RaiseEvent(new AccountOpened(Version, accountId));

        public Account(IEnumerable<IEvent> events) =>
            FeedEvents(events);

        public Account(AccountSnapshot snapshot)
        {
            AccountId = snapshot.AccountId;
            Version = snapshot.EntityVersion;
            foreach (Entry entry in snapshot.Entries)
                _entries.Add(entry);
        }

        public static Account Restore(AccountSnapshot snapshot, IEnumerable<IEvent> stream)
        {
            var account = new Account(snapshot);
            account.FeedEvents(stream);

            return account;
        }

        public static Account Restore(IEnumerable<IEvent> stream) =>
            new(stream);

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

            RaiseEvent(new AmountCredited(Version, AccountId, amount));

            return true;
        }

        private void Apply(AmountCredited @event) =>
            _entries.Add(Entry.Credit(@event.Amount));

        public Result<IError> Debit(Money amount)
        {
            if(Money.Zero == amount)
                return Errors.InvalidAmount;
            if (Money.Zero > Balance - amount)
                return Errors.InsufficientFunds;

            RaiseEvent(new AmountDebited(Version, AccountId, amount));

            return true;
        }

        private void Apply(AmountDebited @event) =>
            _entries.Add(Entry.Debit(@event.Amount));

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

        public AccountSnapshot TakeSnapshot() =>
            new (AccountId, Version, [.. _entries]);
    }
}
