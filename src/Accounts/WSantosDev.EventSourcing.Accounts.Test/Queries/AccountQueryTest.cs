using WSantosDev.EventSourcing.Accounts.Queries;
using WSantosDev.EventSourcing.Commons;

namespace WSantosDev.EventSourcing.Accounts.Test.Queries
{
    public sealed class AccountQueryTest :IDisposable
    {
        private readonly DatabaseSetup _databaseSetup;

        private readonly AccountReadModelStore _accountReadModelStore;

        public AccountQueryTest()
        {
            _databaseSetup = DatabaseSetupFactory.Create();
            _accountReadModelStore = _databaseSetup.ReadModelStore;
        }

        [Fact]
        public async Task Success()
        {
            //Arrange
            AccountId accountId = Guid.NewGuid();
            var account = Account.Open(accountId, 100m).ResultValue;
            await _accountReadModelStore.StoreAsync(new AccountReadModel(accountId, account.Balance));
            var sut = new AccountQuery(_accountReadModelStore);

            //Act
            var stored = await sut.ExecuteAsync(new AccountQueryParams(accountId));

            //Assert
            Assert.True(stored);
            Assert.Equal(100m, stored.Get().Balance);
        }

        [Fact]
        public async Task SuccessButNotFound()
        {
            //Arrange
            var sut = new AccountQuery(_accountReadModelStore);

            //Act
            var stored = await sut.ExecuteAsync(new AccountQueryParams(Guid.NewGuid()));

            //Assert
            Assert.False(stored);
        }

        public void Dispose() =>
            DatabaseSetupDisposer.Dispose(_databaseSetup);
    }
}
