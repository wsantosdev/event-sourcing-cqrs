using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Exchange
{
    public sealed partial class ExchangeOrder : EventBasedEntity
    {
        public AccountId AccountId { get; private set; } = AccountId.Empty;
        public OrderId OrderId { get; private set; } = OrderId.Empty;
        public OrderSide Side { get; private set; } = OrderSide.None;
        public Symbol Symbol { get; private set; } = Symbol.Empty;
        public Quantity Quantity { get; private set; } = Quantity.Zero;
        public Money Price { get; private set; } = Money.Zero;
        public OrderStatus Status { get; private set; } = OrderStatus.None;

        private ExchangeOrder(AccountId accountId, OrderId orderId, OrderSide side,
                              Quantity quantity, Symbol symbol, Money price) =>
            RaiseEvent(new OrderCreated(accountId, orderId, side, quantity, symbol, price, OrderStatus.New));

        private ExchangeOrder(IEnumerable<IEvent> stream) =>
            Hydrate(stream);

        public static ExchangeOrder Restore(IEnumerable<IEvent> stream) =>
            new(stream);

        public static ExchangeOrder Create(AccountId accountId, OrderId orderId, OrderSide side,
                                             Quantity quantity, Symbol symbol, Money price) =>
            new (accountId, orderId, side, quantity, symbol, price);

        private void Apply(OrderCreated @event)
        {
            AccountId = @event.AccountId;
            OrderId = @event.OrderId;
            Side = @event.Side;
            Symbol = @event.Symbol;
            Quantity = @event.Quantity;
            Price = @event.Price;
            Status = @event.OrderStatus;
        }

        public Result<IError> Execute()
        {
            if (Status == OrderStatus.Filled)
                return Errors.AlreadyFilled;

            RaiseEvent(new OrderExecuted(AccountId, OrderId, Side, Quantity, Symbol, Price, OrderStatus.Filled));
            return true;
        }

        private void Apply(OrderExecuted @event) =>
            Status = OrderStatus.Filled;

        protected override void ProcessEvent(IEvent @event)
        {
            switch (@event)
            {
                case OrderCreated created:
                    Apply(created); break;
                case OrderExecuted executed:
                    Apply(executed); break;
            }
        }
    }
}