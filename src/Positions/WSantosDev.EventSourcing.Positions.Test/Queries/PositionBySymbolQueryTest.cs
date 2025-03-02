using WSantosDev.EventSourcing.Positions.Queries;

namespace WSantosDev.EventSourcing.Positions.Test
{
    public sealed class PositionBySymbolQueryTest : IDisposable
    {
        private readonly DatabaseSetup _databaseSetup;

        private readonly PositionReadModelStore _readModelStore;

        public PositionBySymbolQueryTest()
        {
            _databaseSetup = DatabaseSetupFactory.Create();

            _readModelStore = _databaseSetup.ReadModelStore;
        }

        [Fact]
        public async Task Success()
        {
            //Arrange
            var accountId = Guid.NewGuid();
            var symbol = "APPL";
            var available = 10;
            await _readModelStore.StoreAsync(new PositionReadModel(accountId, symbol, available));
            var sut = new PositionBySymbolQuery(_readModelStore);

            //Act
            var stored = await sut.ExecuteAsync(new PositionBySymbolQueryParams(accountId, symbol));

            //Assert
            Assert.True(stored);
            Assert.Equal(available, stored.Get().Available);
        }

        [Fact]
        public async Task SuccessButNotFound()
        {
            //Arrange
            var accountId = Guid.NewGuid();
            var symbol = "APPL";
            var sut = new PositionBySymbolQuery(_readModelStore);

            //Act
            var stored = await sut.ExecuteAsync(new PositionBySymbolQueryParams(accountId, symbol));

            //Assert
            Assert.False(stored);
        }

        public void Dispose() =>
            DatabaseSetupDisposer.Dispose(_databaseSetup);
    }
}
