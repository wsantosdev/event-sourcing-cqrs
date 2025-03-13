using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Commons
{
    public sealed class OrderStatus : ValueWrapper<string>
    {
        public static readonly OrderStatus None = new(nameof(None));
        public static readonly OrderStatus New = new(nameof(New));
        public static readonly OrderStatus Filled = new(nameof(Filled));

        public OrderStatus(string value) : base(value) { }

        public static implicit operator OrderStatus(string value) =>
            value switch
            {
                nameof(New) => New,
                nameof(Filled) => Filled,
                _ => None
            };

        public static implicit operator string(OrderStatus status) =>
            status.Value;

        public override string ToString() =>
            Value;
    }
}
