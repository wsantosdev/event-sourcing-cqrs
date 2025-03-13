using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Orders.Actions;

namespace WSantosDev.EventSourcing.Orders.Test
{
    public sealed class ExecuteTest : IDisposable
    {
        private readonly Database _database;
        private readonly InMemoryMessageBus _messageBus;

        public ExecuteTest()
        {
            _database = DatabaseFactory.Create();
            _messageBus = new InMemoryMessageBus();
        }

        public void Dispose() =>
            DatabaseDisposer.Dispose(_database);

        [Fact]
        public async Task Success()
        {
            //Arrange
            var orderId = Guid.NewGuid();
            Order order = Order.New(Guid.NewGuid(), orderId, OrderSide.Sell, 1, "NVDA", 1m);
            await _database.Store.StoreAsync(order);
            await _database.ViewStore.StoreAsync(OrderView.CreateFrom(order));
            var sut = new Execute(_database.Store, _messageBus);

            //Act
            var executed = await sut.ExecuteAsync(new ExecuteActionParams(orderId));

            //Assert
            Assert.True(executed);
            Assert.Equal(OrderStatus.Filled, (await _database.Store.GetByIdAsync(orderId)).Get().Status);
        }

        [Fact]
        public async Task FailureOrderNotFound()
        {
            //Arrange
            var sut = new Execute(_database.Store, _messageBus);

            //Act
            var executed = await sut.ExecuteAsync(new ExecuteActionParams(Guid.NewGuid()));

            //Assert
            Assert.False(executed);
            Assert.Equal(CommandErrors.OrderNotFound, executed.ErrorValue);
        }

        [Fact]
        public async Task FailureAlreadyExecuted()
        {
            //Arrange
            var orderId = Guid.NewGuid();
            Order order = Order.New(Guid.NewGuid(), orderId, OrderSide.Sell, 1, "NVDA", 1m);
            await _database.Store.StoreAsync(order);
            await _database.ViewStore.StoreAsync(OrderView.CreateFrom(order));
            var sut = new Execute(_database.Store, _messageBus);

            //Act
            await sut.ExecuteAsync(new ExecuteActionParams(orderId));
            var executed = await sut.ExecuteAsync(new ExecuteActionParams(orderId));

            //Assert
            Assert.False(executed);
            Assert.Equal(Errors.AlreadyFilled, executed.ErrorValue);
        }
    }
}
