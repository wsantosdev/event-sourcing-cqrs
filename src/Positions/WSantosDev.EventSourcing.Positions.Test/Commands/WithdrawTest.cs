using WSantosDev.EventSourcing.Positions.Commands;

namespace WSantosDev.EventSourcing.Positions.Test
{
    public sealed class WithdrawTest : IDisposable
    {
        private readonly Database _databaseSetup;

        public WithdrawTest()
        {
            _databaseSetup = DatabaseSetupFactory.Create();
        }

        public void Dispose() =>
            DatabaseSetupDisposer.Dispose(_databaseSetup);

        [Fact]
        public async Task SuccessOpening()
        {
            //Arrange
            var accountId = Guid.NewGuid();
            var symbol = "MSFT";
            var quantity = 10;

            var sut = new Withdraw(_databaseSetup.Store);

            //Act
            var withdrawn = await sut.ExecuteAsync(new WithdrawParams(accountId, symbol, quantity));

            //Assert
            Assert.True(withdrawn);
            Assert.Single(await _databaseSetup.ViewDbContext.ByAccountIdAsync(accountId));
        }

        [Fact]
        public async Task SuccessUpdating()
        {
            //Arrange
            var accountId = Guid.NewGuid();
            var symbol = "MSFT";
            var quantity = 10;
            var position = Position.Open(accountId, symbol, quantity);
            await _databaseSetup.Store.StoreAsync(position);
            await _databaseSetup.ViewStore.StoreAsync(PositionView.CreateFrom(position));

            var sut = new Withdraw(_databaseSetup.Store);

            //Act
            var withdrawn = await sut.ExecuteAsync(new WithdrawParams(accountId, symbol, quantity));

            //Assert
            Assert.True(withdrawn);
            Assert.Single(await _databaseSetup.ViewDbContext.ByAccountIdAsync(accountId));
        }
    }
}
