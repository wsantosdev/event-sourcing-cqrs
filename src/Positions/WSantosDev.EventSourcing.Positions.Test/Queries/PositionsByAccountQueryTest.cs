using WSantosDev.EventSourcing.Positions.Queries;

namespace WSantosDev.EventSourcing.Positions.Test
{
    public sealed class PositionsByAccountQueryTest : IDisposable
    {
        private readonly DatabaseSetup _databaseSetup;

        private readonly PositionReadModelStore _readModelStore;

        public PositionsByAccountQueryTest()
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
            var sut = new PositionsByAccountQuery(_readModelStore);

            //Act
            var stored = await sut.ExecuteAsync(new PositionsByAccountQueryParams(accountId));

            //Assert
            Assert.NotEmpty(stored);
            Assert.Single(stored);
            Assert.Equal(available, stored.First().Available);
        }

        [Fact]
        public async Task SuccessButNotFound()
        {
            //Arrange
            var accountId = Guid.NewGuid();
            var sut = new PositionsByAccountQuery(_readModelStore);

            //Act
            var stored = await sut.ExecuteAsync(new PositionsByAccountQueryParams(accountId));

            //Assert
            Assert.Empty(stored);
        }

        public void Dispose() =>
            DatabaseSetupDisposer.Dispose(_databaseSetup);
    }
}
