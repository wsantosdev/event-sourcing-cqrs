﻿using Moonad;
using WSantosDev.EventSourcing.Commons.Modeling;
using WSantosDev.EventSourcing.Commons;

namespace WSantosDev.EventSourcing.Accounts
{
    public sealed partial class Account : EventBasedEntity
    {
        public Option<AccountId> AccountId { get; private set; } = Option.None<AccountId>();
        
        public Money Balance => _entries.DefaultIfEmpty(Entry.Empty)
                                        .Sum(e => e.Value);

        private readonly IList<Entry> _entries = [];

        private Account(AccountId accountId) =>
            RaiseEvent(new AccountOpened(accountId));

        private Account(IEnumerable<IEvent> events) =>
            Hydrate(events);

        public static Result<Account, IError> Open(AccountId accountId, Money initialDeposit)
        {
            if (accountId == Commons.AccountId.Empty)
                return Errors.EmptyAccountId;

            var account = new Account(accountId);
            
            if (initialDeposit > Money.Zero)
                account.Credit(initialDeposit);

            return account;
        }

        public static Account Restore(IEnumerable<IEvent> events) =>
            new (events);

        private void Apply(AccountOpened accountCreated) =>
            AccountId = accountCreated.AccountId;

        public Result<IError> Credit(Money amount)
        {
            if (amount <= Money.Zero)
                return Result<IError>.Error(Errors.InvalidAmount);

            RaiseEvent(new AmountCredited(amount));

            return true;
        }

        private void Apply(AmountCredited amountCredited) =>
            _entries.Add(Entry.Credit(amountCredited.Amount));

        public Result<IError> Debit(Money amount)
        {
            if(amount == Money.Zero)
                return Errors.InvalidAmount;
            if (Money.Zero < amount)
                return Errors.InsufficientFunds;

            RaiseEvent(new AmountDebited(amount));

            return true;
        }

        private void Apply(AmountDebited amountDebited) =>
            _entries.Add(Entry.Debit(amountDebited.Amount));

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
    }
}
