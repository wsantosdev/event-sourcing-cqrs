using System.Collections.Generic;
using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Orders
{
    public sealed partial class Order : EventBasedEntity
    {
        public AccountId AccountId { get; private set; } = AccountId.Empty;
        public OrderId OrderId { get; private set; } = OrderId.Empty;
        public OrderSide Side { get; private set; } = OrderSide.None;
        public Quantity Quantity { get; private set; } = Quantity.Zero;
        public Symbol Symbol { get; private set; } = Symbol.Empty;
        public Money Price { get; private set; } = Money.Zero;
        public OrderStatus Status { get; private set; } = OrderStatus.None;
        
        private Order(AccountId accountId, OrderId orderId, OrderSide side,
                      Quantity quantity, Symbol symbol, Money price)
        {
            RaiseEvent(new OrderCreated(accountId, orderId, side, quantity, symbol, price));
        }

        private void Apply(OrderCreated @event)
        {
            AccountId = @event.AccountId;
            OrderId = @event.OrderId;
            Side = @event.Side;
            Symbol = @event.Symbol;
            Quantity = @event.Quantity;
            Price = @event.Price;
            Status = OrderStatus.New;
        }

        public Order(IEnumerable<IEvent> stream) =>
            Restore(stream);

        public static Result<Order, IError> New(AccountId accountId, OrderId orderId, OrderSide side,
                                                Quantity quantity, Symbol symbol, Money price)
        {
            if(AccountId.Empty == accountId)
                return Errors.EmptyAccountId;
            if (OrderId.Empty == orderId)
                return Errors.EmptyOrderId;
            if (OrderSide.None == side)
                return Errors.InvalidSide;
            if (Quantity.Zero >= quantity)
                return Errors.InvalidQuantity;
            if (Symbol.Empty == symbol)
                return Errors.InvalidSymbol;
            if (Money.Zero >= price)
                return Errors.InvalidPrice;

            return new Order(accountId, orderId, side, quantity, symbol, price);
        }

        public Result<IError> Execute()
        {
            if (Status == OrderStatus.Filled)
                return Errors.AlreadyFilled;
            
            RaiseEvent(new OrderExecuted(AccountId, OrderId, Side, Quantity, Symbol, Price));
            
            return true;
        }

        private void Apply(OrderExecuted @event) =>
            Status = OrderStatus.Filled;

        protected override void ProcessEvent(IEvent @event)
        {
            switch (@event)
            {
                case OrderCreated orderCreated:
                    Apply(orderCreated); break;
                case OrderExecuted orderExecuted:
                    Apply(orderExecuted); break;
            }
        }
    }
}