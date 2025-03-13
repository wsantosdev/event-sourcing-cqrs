using WSantosDev.EventSourcing.Accounts.Commands;
using WSantosDev.EventSourcing.Commons;

namespace WSantosDev.EventSourcing.Accounts.Test
{
    public sealed class CreditTest : IDisposable
    {
        private readonly Database _databaseSetup;

        private readonly AccountViewStore _accountReadModelStore;
        private readonly AccountStore _accountStore;

        public CreditTest()
        {
            _databaseSetup = DatabaseFactory.Create();

            _accountReadModelStore = _databaseSetup.ViewStore;
            _accountStore = _databaseSetup.Store;
        }

        [Fact]
        public async Task Success()
        {
            //Arrange
            AccountId accountId = Guid.NewGuid();
            var account = Account.Open(accountId, 0m).ResultValue;
            await _accountStore.StoreAsync(account);
            var expectedCredit = 10m;
            var sut = new Credit(_accountStore);

            //Act
            var credited = await sut.ExecuteAsync(new CreditParams(accountId, expectedCredit));

            //Assert
            Assert.True(credited);
            Assert.Equal<decimal>(expectedCredit, (await _accountStore.ByIdAsync(accountId)).Get().Balance);
            Assert.Equal(expectedCredit, (await _accountReadModelStore.ByIdAsync(accountId)).Get().Balance);
        }

        [Fact]
        public async Task FailureInvalidAmount()
        {
            //Arrange
            AccountId accountId = Guid.NewGuid();
            var expectedBalance = 1m;
            var account = Account.Open(accountId, expectedBalance).ResultValue;
            await _accountStore.StoreAsync(account);
            await _accountReadModelStore.StoreAsync(new AccountView(accountId, account.Balance));
            var sut = new Credit(_accountStore);

            //Act
            var credited = await sut.ExecuteAsync(new CreditParams(accountId, 0m));

            //Assert
            Assert.False(credited);
            Assert.Equal(Errors.InvalidAmount, credited.ErrorValue);
            Assert.Equal(expectedBalance, (await _accountReadModelStore.ByIdAsync(accountId)).Get().Balance);
        }

        public void Dispose() =>
            DatabaseDisposer.Dispose(_databaseSetup);
    }
}
