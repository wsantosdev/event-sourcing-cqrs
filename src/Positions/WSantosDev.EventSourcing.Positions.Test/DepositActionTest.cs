using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Positions.Commands;
using WSantosDev.EventSourcing.Positions.DomainEvents;

namespace WSantosDev.EventSourcing.Positions.Test
{
    public sealed class DepositActionTest : IDisposable
    {
        private readonly DatabaseSetup _databaseSetup;

        private readonly PositionStore _positionStore;
        private readonly PositionReadModelStore _readModelStore;

        private readonly InMemoryMessageBus _messageBus;

        public DepositActionTest()
        {
            _databaseSetup = DatabaseSetupFactory.Create();

            _positionStore = _databaseSetup.Store;
            _readModelStore = _databaseSetup.ReadModelStore;

            _messageBus = new InMemoryMessageBus();
            _messageBus.Subscribe(new DepositedHandler(_readModelStore));
        }

        [Fact]
        public async Task Success()
        {
            //Arrange
            var accountId = Guid.NewGuid();
            var symbol = "MSFT";
            var quantity = 10;

            var sut = new Deposit(_positionStore, _messageBus);

            //Act
            var deposited = await sut.ExecuteAsync(new DepositParams(accountId, symbol, quantity));

            //Assert
            Assert.True(deposited);
            Assert.Single(await _readModelStore.GetByAccountAsync(accountId));

        }


        public void Dispose() =>
            DatabaseSetupDisposer.Dispose(_databaseSetup);
    }
}
