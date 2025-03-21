using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Accounts
{
    public sealed class Entry : ValueWrapper<decimal>
    {
        public static readonly Entry Empty = new(0);
                
        public Entry(decimal value) : base(value) { }

        public Entry() : base(0) { }
        
        public static Entry Credit(Money amount)
        {
            return new Entry(amount.Value);
        }

        public static Entry Debit(Money amount)
        {
            return new Entry(amount.Value * -1);
        }
    }
}
