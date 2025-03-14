using WSantosDev.EventSourcing.Positions.Queries;

namespace WSantosDev.EventSourcing.Positions.Test
{
    public sealed class PositionsByAccountQueryTest : IDisposable
    {
        private readonly Database _database;

        public PositionsByAccountQueryTest()
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
            var sut = new PositionsByAccount(_database.ViewDbContext);

            //Act
            var stored = await sut.ExecuteAsync(new PositionsByAccountParams(accountId));

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
            var sut = new PositionsByAccount(_database.ViewDbContext);

            //Act
            var stored = await sut.ExecuteAsync(new PositionsByAccountParams(accountId));

            //Assert
            Assert.Empty(stored);
        }

        public void Dispose() =>
            DatabaseSetupDisposer.Dispose(_database);
    }
}
