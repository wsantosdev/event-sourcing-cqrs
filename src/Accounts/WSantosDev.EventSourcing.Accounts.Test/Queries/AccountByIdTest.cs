using WSantosDev.EventSourcing.Accounts.Queries;
using WSantosDev.EventSourcing.Commons;

namespace WSantosDev.EventSourcing.Accounts.Test.Queries
{
    public sealed class AccountByIdTest :IDisposable
    {
        private readonly Database _database;

        public AccountByIdTest()
        {
            _database = DatabaseFactory.Create();
        }

        [Fact]
        public async Task Success()
        {
            //Arrange
            AccountId accountId = Guid.NewGuid();
            var initialDeposit = 100m;
            var account = Account.Open(accountId, initialDeposit).ResultValue;
            await _database.ViewStore.StoreAsync(new AccountView(accountId, account.Balance));
            var sut = new AccountById(_database.ViewDbContext);

            //Act
            var stored = await sut.ExecuteAsync(new AccountByIdParams(accountId));

            //Assert
            Assert.True(stored);
            Assert.Equal(initialDeposit, stored.Get().Balance);
        }

        [Fact]
        public async Task SuccessButNotFound()
        {
            //Arrange
            var sut = new AccountById(_database.ViewDbContext);

            //Act
            var stored = await sut.ExecuteAsync(new AccountByIdParams(Guid.NewGuid()));

            //Assert
            Assert.False(stored);
        }

        public void Dispose() =>
            DatabaseDisposer.Dispose(_database);
    }
}
