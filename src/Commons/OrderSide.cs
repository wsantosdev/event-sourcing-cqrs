using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Commons
{
    public sealed class OrderSide : ValueWrapper<string>
    {
        public static readonly OrderSide None = new(nameof(None));
        public static readonly OrderSide Buy = new(nameof(Buy));
        public static readonly OrderSide Sell = new(nameof(Sell));

        public OrderSide(string value) : base(value) { }

        public static implicit operator OrderSide(string value) =>
            value switch
            {
                nameof(Buy) => Buy,
                nameof(Sell) => Sell,
                _ => None
            };

        public static implicit operator string(OrderSide side) =>
            side.Value;

        public static bool operator ==(string left, OrderSide right) =>
            left == right.Value;

        public static bool operator !=(string left, OrderSide right) =>
            !(left == right);

        public override bool Equals(object? obj)
        {
            if (obj is null || obj is not string)
                return false;

            return base.Equals(obj);
        }

        public override int GetHashCode() 
            => base.GetHashCode();
    }
}
