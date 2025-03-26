using WSantosDev.EventSourcing.Accounts.Commands;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Accounts.Test
{
    public sealed class CreditTest : IDisposable
    {
        private readonly Database _database;
        private readonly IMessageBus _messageBus;

        public CreditTest()
        {
            _database = DatabaseFactory.Create();
            _messageBus = new InMemoryMessageBus();
        }

        [Fact]
        public async Task Success()
        {
            //Arrange
            AccountId accountId = Guid.NewGuid();
            var account = Account.Open(accountId, 0m).ResultValue;
            await _database.Store.StoreAsync(account);
            var expectedCredit = 10m;
            var sut = new Credit(_database.Store, _messageBus);

            //Act
            var credited = await sut.ExecuteAsync(new CreditParams(accountId, expectedCredit));

            //Assert
            Assert.True(credited);
            Assert.Equal<decimal>(expectedCredit, (await _database.Store.ByIdAsync(accountId)).Get().Balance);
        }

        [Fact]
        public async Task FailureInvalidAmount()
        {
            //Arrange
            AccountId accountId = Guid.NewGuid();
            var expectedBalance = 1m;
            var account = Account.Open(accountId, expectedBalance).ResultValue;
            await _database.Store.StoreAsync(account);
            await _database.ViewStore.StoreAsync(new AccountView(accountId, account.Balance));
            var sut = new Credit(_database.Store, _messageBus);

            //Act
            var credited = await sut.ExecuteAsync(new CreditParams(accountId, 0m));

            //Assert
            Assert.False(credited);
            Assert.Equal(Errors.InvalidAmount, credited.ErrorValue);
        }

        public void Dispose() =>
            DatabaseDisposer.Dispose(_database);
    }
}
