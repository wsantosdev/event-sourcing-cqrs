using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Commons
{
    public sealed class Money : ValueWrapper<decimal>
    {
        public readonly static Money Zero = new(0);

        private Money(decimal value) : base(value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Only non-negative values are allowed to represent Money.");
        }

        public static implicit operator Money(decimal value) =>
            new(value);

        public static implicit operator decimal(Money value) =>
            value.Value;

        public static bool operator ==(Money left, decimal right) =>
            left.Value.Equals(right);

        public static bool operator !=(Money left, decimal right) =>
            left.Value.Equals(right);

        public static bool operator >(Money left, Money right) =>
            left.Value > right.Value;

        public static bool operator <(Money left, Money right) =>
            left.Value < right.Value;

        public static bool operator >(Money left, decimal right) =>
            left.Value > right;

        public static bool operator <(Money left, decimal right) =>
            left.Value < right;

        public override bool Equals(object? obj) =>
            base.Equals(obj);

        public override int GetHashCode() =>
            base.GetHashCode();
    }
}
