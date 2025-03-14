using WSantosDev.EventSourcing.Positions.Queries;

namespace WSantosDev.EventSourcing.Positions.Test
{
    public sealed class PositionBySymbolQueryTest : IDisposable
    {
        private readonly Database _database;

        public PositionBySymbolQueryTest()
        {
            _database = DatabaseSetupFactory.Create();
        }

        [Fact]
        public async Task Success()
        {
            //Arrange
            var accountId = Guid.NewGuid();
            var symbol = "APPL";
            var available = 10;
            await _database.ViewStore.StoreAsync(PositionView.CreateFrom(Position.Open(accountId, symbol, available)));
            var sut = new PositionBySymbol(_database.ViewDbContext);

            //Act
            var stored = await sut.ExecuteAsync(new PositionBySymbolParams(accountId, symbol));

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
            var sut = new PositionBySymbol(_database.ViewDbContext);

            //Act
            var stored = await sut.ExecuteAsync(new PositionBySymbolParams(accountId, symbol));

            //Assert
            Assert.False(stored);
        }

        public void Dispose() =>
            DatabaseSetupDisposer.Dispose(_database);
    }
}
