using WSantosDev.EventSourcing.Accounts.Commands;
using WSantosDev.EventSourcing.Commons;

namespace WSantosDev.EventSourcing.Accounts.Test
{
    public sealed class DebitTest : IDisposable
    {
        private readonly Database _database;

        public DebitTest()
        {
            _database = DatabaseFactory.Create();
        }

        [Fact]
        public async Task Success()
        {
            //Arrange
            AccountId accountId = Guid.NewGuid();
            var account = Account.Open(accountId, 100m).ResultValue;
            await _database.Store.StoreAsync(account);
            var sut = new Debit(_database.Store);

            //Act
            var debited = await sut.ExecuteAsync(new DebitParams(accountId, 10m));

            //Assert
            Assert.True(debited);
            Assert.Equal<decimal>(90m, (await _database.Store.ByIdAsync(accountId)).Get().Balance);
            Assert.Equal(90m, (await _database.ViewDbContext.ByAccountIdAsync(accountId)).Get().Balance);
        }

        [Fact]
        public async Task FailureInvalidAmount()
        {
            //Arrange
            AccountId accountId = Guid.NewGuid();
            var initialDeposit = 100m;
            var account = Account.Open(accountId, initialDeposit).ResultValue;
            await _database.Store.StoreAsync(account);
            await _database.ViewStore.StoreAsync(new AccountView(accountId, account.Balance));
            var sut = new Debit(_database.Store);

            //Act
            var debited = await sut.ExecuteAsync(new DebitParams(accountId, 0m));

            //Assert
            Assert.False(debited);
            Assert.Equal(Errors.InvalidAmount, debited.ErrorValue);
            Assert.Equal<decimal>(initialDeposit, (await _database.Store.ByIdAsync(accountId)).Get().Balance);
            Assert.Equal(initialDeposit, (await _database.ViewDbContext.ByAccountIdAsync(accountId)).Get().Balance);
        }
                
        [Fact]
        public async Task FailureInsuficientFunds()
        {
            //Arrange
            AccountId accountId = Guid.NewGuid();
            var account = Account.Open(accountId, 100m).ResultValue;
            await _database.Store.StoreAsync(account);
            await _database.ViewStore.StoreAsync(new AccountView(accountId, account.Balance));
            var sut = new Debit(_database.Store);

            //Act
            var debited = await sut.ExecuteAsync(new DebitParams(accountId, 200m));

            //Assert
            Assert.False(debited);
            Assert.Equal(Errors.InsufficientFunds, debited.ErrorValue);
            Assert.Equal<decimal>(100m, (await _database.Store.ByIdAsync(accountId)).Get().Balance);
            Assert.Equal(100m, (await _database.ViewDbContext.ByAccountIdAsync(accountId)).Get().Balance);
        }

        public void Dispose() =>
            DatabaseDisposer.Dispose(_database);
    }
}
