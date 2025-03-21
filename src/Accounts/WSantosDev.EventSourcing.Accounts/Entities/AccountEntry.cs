using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Accounts
{
    public sealed class AccountEntry : ValueWrapper<decimal>
    {
        public static readonly AccountEntry Empty = new(0);
                
        public AccountEntry(decimal value) : base(value) { }

        public AccountEntry() : base(0) { }
        
        public static AccountEntry Credit(Money amount)
        {
            return new AccountEntry(amount.Value);
        }

        public static AccountEntry Debit(Money amount)
        {
            return new AccountEntry(amount.Value * -1);
        }
    }
}
