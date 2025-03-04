using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Exchange.Commands;
using WSantosDev.EventSourcing.Exchange.DomainEvents;

namespace WSantosDev.EventSourcing.Exchange.Test
{
    public sealed class CreateTest : IDisposable
    {
        private readonly DatabaseSetup _databaseSetup;

        private readonly ExchangeOrderReadModelStore _readModelStore;
        private readonly ExchangeOrderStore _store;

        private readonly InMemoryMessageBus _messageBus;

        public CreateTest()
        {
            _databaseSetup = DatabaseSetupFactory.Create();

            _readModelStore = _databaseSetup.ReadModelStore;
            _store = _databaseSetup.Store;

            _messageBus = new InMemoryMessageBus();
            _messageBus.Subscribe(new ExchangeOrderCreatedHandler(_readModelStore));
        }

        [Fact]
        public async Task Success() 
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var sut = new Create(_store, _messageBus);

            //Act
            var created = await sut.ExecuteAsync(new CreateActionParams(Guid.NewGuid(), orderId, OrderSide.Buy, 10, "NVDA", 10m));

            //Assert
            Assert.True(created);
            Assert.True(await _store.GetByIdAsync(orderId));
            Assert.NotEmpty((await _readModelStore.AllAsync()).Select(o => o.OrderId == orderId));
        }

        public void Dispose() =>
            DatabaseSetupDisposer.Dispose(_databaseSetup);
    }
}
