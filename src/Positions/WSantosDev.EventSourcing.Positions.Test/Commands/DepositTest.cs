using WSantosDev.EventSourcing.Positions.Commands;

namespace WSantosDev.EventSourcing.Positions.Test
{
    public sealed class DepositTest : IDisposable
    {
        private readonly Database _database;

        public DepositTest()
        {
            _database = DatabaseFactory.Create();
        }

        public void Dispose() =>
            DatabaseDisposer.Dispose(_database);

        [Fact]
        public async Task SuccessOpening()
        {
            //Arrange
            var accountId = Guid.NewGuid();
            var symbol = "MSFT";
            var quantity = 10;

            var sut = new Deposit(_database.Store);

            //Act
            var deposited = await sut.ExecuteAsync(new DepositParams(accountId, symbol, quantity));

            //Assert
            Assert.True(deposited);
            Assert.Single(await _database.ViewDbContext.ByAccountIdAsync(accountId));
        }

        [Fact]
        public async Task SuccessUpdating()
        {
            //Arrange
            var accountId = Guid.NewGuid();
            var symbol = "MSFT";
            var quantity = 10;
            var position = Position.Open(accountId, symbol, quantity);
            await _database.Store.StoreAsync(position);
            await _database.ViewStore.StoreAsync(PositionView.CreateFrom(position));
            
            var sut = new Deposit(_database.Store);

            //Act
            var deposited = await sut.ExecuteAsync(new DepositParams(accountId, symbol, quantity));

            //Assert
            Assert.True(deposited);
            Assert.Single(await _database.ViewDbContext.ByAccountIdAsync(accountId));
            Assert.Equal(quantity * 2, (await _database.ViewDbContext.BySymbolAsync(accountId, symbol)).Get().Available);
        }
    }
}
