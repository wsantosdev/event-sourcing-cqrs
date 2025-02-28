using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Orders.Actions;
using WSantosDev.EventSourcing.Orders.DomainEvents;

namespace WSantosDev.EventSourcing.Orders.Test
{
    public sealed class ExecuteActionTest : IDisposable
    {
        private readonly DatabaseSetup _databaseSetup;

        private readonly OrderReadModelStore _readModelStore;
        private readonly OrderStore _store;

        private readonly InMemoryMessageBus _messageBus;

        public ExecuteActionTest()
        {
            _databaseSetup = DatabaseSetupFactory.Create();

            _readModelStore = _databaseSetup.ReadModelStore;
            _store = _databaseSetup.Store;

            _messageBus = new InMemoryMessageBus();
            _messageBus.Subscribe(new OrderExecutedHandler(_readModelStore));
        }

        [Fact]
        public void Success()
        {
            //Arrange
            var orderId = Guid.NewGuid();
            Order order = Order.New(Guid.NewGuid(), orderId, OrderSide.Sell, 1, "NVDA", 1m);
            _store.Store(order);
            _readModelStore.Store(new OrderReadModel(order.AccountId, orderId, order.Side, order.Quantity, 
                                                     order.Symbol, order.Price, OrderStatus.New));
            var sut = new ExecuteAction(_store, _messageBus);

            //Act
            var executed = sut.Execute(new ExecuteActionParams(orderId));

            //Assert
            Assert.True(executed);
            Assert.Equal(OrderStatus.Filled, _store.GetById(orderId).Get().Status);
        }

        [Fact]
        public void FailureOrderNotFound()
        {
            //Arrange
            var sut = new ExecuteAction(_store, _messageBus);

            //Act
            var executed = sut.Execute(new ExecuteActionParams(Guid.NewGuid()));

            //Assert
            Assert.False(executed);
            Assert.Equal(ActionErrors.OrderNotFound, executed.ErrorValue);
        }

        [Fact]
        public void FailureAlreadyExecuted()
        {
            //Arrange
            var orderId = Guid.NewGuid();
            Order order = Order.New(Guid.NewGuid(), orderId, OrderSide.Sell, 1, "NVDA", 1m);
            _store.Store(order);
            _readModelStore.Store(new OrderReadModel(order.AccountId, orderId, order.Side, order.Quantity,
                                                     order.Symbol, order.Price, OrderStatus.New));
            var sut = new ExecuteAction(_store, _messageBus);

            //Act
            sut.Execute(new ExecuteActionParams(orderId));
            var executed = sut.Execute(new ExecuteActionParams(orderId));

            //Assert
            Assert.False(executed);
            Assert.Equal(Errors.AlreadyFilled, executed.ErrorValue);
        }

        public void Dispose() =>
            DatabaseSetupDisposer.Dispose(_databaseSetup);
    }
}
