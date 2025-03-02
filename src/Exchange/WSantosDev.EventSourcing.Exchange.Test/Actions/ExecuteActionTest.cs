using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Exchange.Actions;
using WSantosDev.EventSourcing.Exchange.DomainEvents;

namespace WSantosDev.EventSourcing.Exchange.Test.Actions
{
    public sealed class ExecuteActionTest : IDisposable
    {
        private readonly DatabaseSetup _databaseSetup;

        private readonly ExchangeOrderReadModelStore _readModelStore;
        private readonly ExchangeOrderStore _store;

        private readonly InMemoryMessageBus _messageBus;

        public ExecuteActionTest()
        {
            _databaseSetup = DatabaseSetupFactory.Create();

            _readModelStore = _databaseSetup.ReadModelStore;
            _store = _databaseSetup.Store;

            _messageBus = new InMemoryMessageBus();
            _messageBus.Subscribe(new ExchangeOrderExecutedHandler(_readModelStore));
        }

        [Fact]
        public async Task Success()
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var order = ExchangeOrder.Create(Guid.NewGuid(), orderId, OrderSide.Buy, 100, "MSFT", 10m);
            await _store.StoreAsync(order);
            await _readModelStore.StoreAsync(new ExchangeOrderReadModel(order.AccountId, order.OrderId, order.Side, order.Quantity, 
                                                             order.Symbol, order.Price, order.Status));
            
            var sut = new ExecuteAction(_store, _messageBus);

            //Act
            var executed = await sut.ExecuteAsync(new ExecuteActionParams(orderId));

            //Assert
            Assert.True(executed);
            Assert.Equal(OrderStatus.Filled, (await _readModelStore.GetAllAsync()).First(o => o.OrderId == orderId).Status);
        }

        [Fact]
        public async Task FailureAlreadyFilled()
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var order = ExchangeOrder.Create(Guid.NewGuid(), orderId, OrderSide.Buy, 100, "MSFT", 10m);
            order.Execute();
            await _store.StoreAsync(order);

            var sut = new ExecuteAction(_store, _messageBus);

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
            var sut = new ExecuteAction(_store, _messageBus);

            //Act
            var executed = await sut.ExecuteAsync(new ExecuteActionParams(Guid.NewGuid()));

            //Assert
            Assert.False(executed);
            Assert.Equal(ActionErrors.OrderNotFound, executed.ErrorValue);
        }

        public void Dispose() =>
            DatabaseSetupDisposer.Dispose(_databaseSetup);
    }
}
