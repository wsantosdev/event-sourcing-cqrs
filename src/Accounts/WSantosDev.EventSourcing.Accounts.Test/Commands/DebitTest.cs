using WSantosDev.EventSourcing.Accounts.Commands;
using WSantosDev.EventSourcing.Commons;

namespace WSantosDev.EventSourcing.Accounts.Test
{
    public sealed class DebitTest : IDisposable
    {
        private readonly Database _databaseSetup;

        private readonly AccountViewStore _accountReadModelStore;
        private readonly AccountStore _accountStore;

        public DebitTest()
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
            var account = Account.Open(accountId, 100m).ResultValue;
            await _accountStore.StoreAsync(account);
            var sut = new Debit(_accountStore);

            //Act
            var debited = await sut.ExecuteAsync(new DebitParams(accountId, 10m));

            //Assert
            Assert.True(debited);
            Assert.Equal<decimal>(90m, (await _accountStore.ByIdAsync(accountId)).Get().Balance);
            Assert.Equal(90m, (await _accountReadModelStore.ByIdAsync(accountId)).Get().Balance);
        }

        [Fact]
        public async Task FailureInvalidAmount()
        {
            //Arrange
            AccountId accountId = Guid.NewGuid();
            var initialDeposit = 100m;
            var account = Account.Open(accountId, initialDeposit).ResultValue;
            await _accountStore.StoreAsync(account);
            await _accountReadModelStore.StoreAsync(new AccountView(accountId, account.Balance));
            var sut = new Debit(_accountStore);

            //Act
            var debited = await sut.ExecuteAsync(new DebitParams(accountId, 0m));

            //Assert
            Assert.False(debited);
            Assert.Equal(Errors.InvalidAmount, debited.ErrorValue);
            Assert.Equal<decimal>(initialDeposit, (await _accountStore.ByIdAsync(accountId)).Get().Balance);
            Assert.Equal(initialDeposit, (await _accountReadModelStore.ByIdAsync(accountId)).Get().Balance);
        }
                
        [Fact]
        public async Task FailureInsuficientFunds()
        {
            //Arrange
            AccountId accountId = Guid.NewGuid();
            var account = Account.Open(accountId, 100m).ResultValue;
            await _accountStore.StoreAsync(account);
            await _accountReadModelStore.StoreAsync(new AccountView(accountId, account.Balance));
            var sut = new Debit(_accountStore);

            //Act
            var debited = await sut.ExecuteAsync(new DebitParams(accountId, 200m));

            //Assert
            Assert.False(debited);
            Assert.Equal(Errors.InsufficientFunds, debited.ErrorValue);
            Assert.Equal<decimal>(100m, (await _accountStore.ByIdAsync(accountId)).Get().Balance);
            Assert.Equal(100m, (await _accountReadModelStore.ByIdAsync(accountId)).Get().Balance);
        }

        public void Dispose() =>
            DatabaseDisposer.Dispose(_databaseSetup);
    }
}
