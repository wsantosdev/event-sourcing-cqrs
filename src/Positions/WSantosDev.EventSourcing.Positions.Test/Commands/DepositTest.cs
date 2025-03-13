using WSantosDev.EventSourcing.Positions.Commands;

namespace WSantosDev.EventSourcing.Positions.Test
{
    public sealed class DepositTest : IDisposable
    {
        private readonly Database _databaseSetup;

        public DepositTest()
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

            var sut = new Deposit(_databaseSetup.Store);

            //Act
            var deposited = await sut.ExecuteAsync(new DepositParams(accountId, symbol, quantity));

            //Assert
            Assert.True(deposited);
            Assert.Single(await _databaseSetup.ViewDbContext.ByAccountIdAsync(accountId));
        }

        [Fact]
        public async Task SuccessAlreadyOpen()
        {
            //Arrange
            var accountId = Guid.NewGuid();
            var symbol = "MSFT";
            var quantity = 10;
            var position = Position.Open(accountId, symbol, quantity);
            await _databaseSetup.Store.StoreAsync(position);
            await _databaseSetup.ViewStore.StoreAsync(PositionView.CreateFrom(position));
            
            var sut = new Deposit(_databaseSetup.Store);

            //Act
            var deposited = await sut.ExecuteAsync(new DepositParams(accountId, symbol, quantity));

            //Assert
            Assert.True(deposited);
            Assert.Single(await _databaseSetup.ViewDbContext.ByAccountIdAsync(accountId));
        }
    }
}
