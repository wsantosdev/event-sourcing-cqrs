using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Exchange.Commands;

namespace WSantosDev.EventSourcing.Exchange.Test
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

        [Fact]
        public async Task Success()
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var order = ExchangeOrder.Create(Guid.NewGuid(), orderId, OrderSide.Buy, 100, "MSFT", 10m);
            await _database.Store.StoreAsync(order);
            await _database.ViewStore.StoreAsync(ExchangeOrderView.CreateFrom(order));
            
            var sut = new Execute(_database.Store, _messageBus);

            //Act
            var executed = await sut.ExecuteAsync(new ExecuteActionParams(orderId));

            //Assert
            Assert.True(executed);
            Assert.Equal(OrderStatus.Filled, (await _database.ViewDbContext.ByOrderIdAsync(orderId)).Get().Status);
        }

        [Fact]
        public async Task FailureAlreadyFilled()
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var order = ExchangeOrder.Create(Guid.NewGuid(), orderId, OrderSide.Buy, 100, "MSFT", 10m);
            order.Execute();
            await _database.Store.StoreAsync(order);

            var sut = new Execute(_database.Store, _messageBus);

            //Act
            var executed = await sut.ExecuteAsync(new ExecuteActionParams(orderId));

            //Assert
            Assert.False(executed);
            Assert.Equal(Errors.AlreadyFilled, executed.ErrorValue);
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
            Assert.Equal(CommandsErrors.OrderNotFound, executed.ErrorValue);
        }

        public void Dispose() =>
            DatabaseDisposer.Dispose(_database);
    }
}
