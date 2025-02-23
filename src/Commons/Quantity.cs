using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Commons
{
    public sealed class Quantity : ValueWrapper<int>
    {
        public static readonly Quantity Zero = 0;

        public Quantity(int value) : base(value) {}

        public static implicit operator Quantity(int value) =>
            new(value);

        public static implicit operator int(Quantity quantity) =>
            quantity.Value;

        public static Quantity operator -(Quantity left, Quantity right) =>
            (left.Value - right.Value);

        public static Quantity operator -(Quantity left, decimal right) =>
            (Quantity)(left.Value - right);

        public static Quantity operator +(Quantity left, Quantity right) =>
            checked(left.Value + right.Value);

        public static Quantity operator +(Quantity left, decimal right) =>
            (Quantity)checked(left.Value + right);

        public static bool operator >(Quantity left, Quantity right) =>
            left.Value > right.Value;

        public static bool operator <(Quantity left, Quantity right) =>
            left.Value < right.Value;
    }
}
