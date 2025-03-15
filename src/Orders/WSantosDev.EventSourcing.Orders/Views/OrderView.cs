using System;

namespace WSantosDev.EventSourcing.Orders
{
    public class OrderView
    {
        public Guid AccountId { get; private set; }
        public Guid OrderId { get; private set; }
        public string Side { get; private set; } = string.Empty;
        public int Quantity { get; private set; }
        public string Symbol { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public string Status { get; private set; } = string.Empty;

        private OrderView() { }

        private OrderView(Order order)
        { 
            AccountId = order.AccountId;
            OrderId = order.OrderId;
            Side = order.Side;
            Quantity = order.Quantity;
            Symbol = order.Symbol;
            Price = order.Price;
            Status = order.Status;
        }

        public static OrderView CreateFrom(Order source) =>
            new (source);

        public void UpdateFrom(Order source) =>
            Status = source.Status;
    }
}
