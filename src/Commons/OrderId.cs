namespace WSantosDev.EventSourcing.Commons
{
    public sealed class OrderId
    {
        public static readonly OrderId Empty = new(Guid.Empty);

        public Guid Value { get; init; }

        private OrderId(Guid value) =>
            Value = value;

        public OrderId() { }

        public static OrderId New() => 
            new (Guid.NewGuid());

        public static implicit operator OrderId(Guid guid) =>
            new(guid);

        public static implicit operator Guid(OrderId orderId) =>
            orderId.Value;

        public override string ToString() =>
            Value.ToString();
    }
}