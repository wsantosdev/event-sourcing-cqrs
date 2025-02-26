using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Exchange.Actions;
using WSantosDev.EventSourcing.Exchange.DomainEvents;

namespace WSantosDev.EventSourcing.Exchange.Test
{
    public sealed class CreateActionTest : IDisposable
    {
        private readonly DatabaseSetup _databaseSetup;

        private readonly ExchangeOrderReadModelStore _readModelStore;
        private readonly ExchangeOrderStore _store;

        private readonly InMemoryMessageBus _messageBus;

        public CreateActionTest()
        {
            _databaseSetup = DatabaseSetupFactory.Create();

            _readModelStore = _databaseSetup.ReadModelStore;
            _store = _databaseSetup.Store;

            _messageBus = new InMemoryMessageBus();
            _messageBus.Subscribe(new ExchangeOrderCreatedHandler(_readModelStore));
        }

        [Fact]
        public void Success() 
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var sut = new CreateAction(_store, _messageBus);

            //Act
            var created = sut.Execute(new CreateActionParams(Guid.NewGuid(), orderId, OrderSide.Buy, 10, "NVDA", 10m));

            //Assert
            Assert.True(created);
            Assert.True(_store.GetById(orderId));
            Assert.NotEmpty(_readModelStore.GetAll().Select(o => o.OrderId == orderId));
        }

        public void Dispose() =>
            DatabaseSetupDisposer.Dispose(_databaseSetup);
    }
}
