namespace WSantosDev.EventSourcing.Exchange
{
    public class ExchangeOrderView
    {
        public Guid AccountId { get; private set; }
        public Guid OrderId { get; private set; }
        public string Side { get; private set; } = string.Empty;
        public int Quantity { get; private set; }
        public string Symbol { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public string Status { get; private set; } = string.Empty;

        private ExchangeOrderView() { }

        private ExchangeOrderView(ExchangeOrder source)
        {
            AccountId = source.AccountId;
            OrderId = source.OrderId;
            Side = source.Side;
            Quantity = source.Quantity;
            Symbol = source.Symbol;
            Price = source.Price;
            Status = source.Status;
        }

        public static ExchangeOrderView CreateFrom(ExchangeOrder source) =>
            new(source);

        public void UpdateFrom(ExchangeOrder source) =>
            Status = source.Status;

        public void UpdateFrom(ExchangeOrderView source) =>
            Status = source.Status;
    }
}
