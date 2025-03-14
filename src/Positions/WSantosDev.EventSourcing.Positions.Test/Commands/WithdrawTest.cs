using WSantosDev.EventSourcing.Positions.Commands;

namespace WSantosDev.EventSourcing.Positions.Test
{
    public sealed class WithdrawTest : IDisposable
    {
        private readonly Database _database;

        public WithdrawTest()
        {
            _database = DatabaseFactory.Create();
        }

        public void Dispose() =>
            DatabaseDisposer.Dispose(_database);

        [Fact]
        public async Task Success()
        {
            //Arrange
            var accountId = Guid.NewGuid();
            var symbol = "MSFT";
            var quantity = 10;
            var position = Position.Open(accountId, symbol, quantity);
            await _database.Store.StoreAsync(position);

            var sut = new Withdraw(_database.Store);

            //Act
            var withdrawn = await sut.ExecuteAsync(new WithdrawParams(accountId, symbol, quantity));

            //Assert
            Assert.True(withdrawn);
            Assert.Equal(0m, (await _database.ViewDbContext.BySymbolAsync(accountId, symbol)).Get().Available);
        }
    }
}
