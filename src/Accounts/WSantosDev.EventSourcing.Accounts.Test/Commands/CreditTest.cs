using WSantosDev.EventSourcing.Accounts.Commands;
using WSantosDev.EventSourcing.Accounts.DomainEvents;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Accounts.Test
{
    public sealed class CreditTest : IDisposable
    {
        private readonly DatabaseSetup _databaseSetup;

        private readonly AccountReadModelStore _accountReadModelStore;
        private readonly AccountStore _accountStore;

        private readonly InMemoryMessageBus _messageBus;

        public CreditTest()
        {
            _databaseSetup = DatabaseSetupFactory.Create();

            _accountReadModelStore = _databaseSetup.ReadModelStore;
            _accountStore = _databaseSetup.Store;

            _messageBus = new InMemoryMessageBus();
            _messageBus.Subscribe(new AccountUpdatedHandler(_accountReadModelStore));
        }

        [Fact]
        public async Task Success()
        {
            //Arrange
            AccountId accountId = Guid.NewGuid();
            var account = Account.Open(accountId, 0m).ResultValue;
            await _accountStore.StoreAsync(account);
            await _accountReadModelStore.StoreAsync(new AccountReadModel(accountId, account.Balance));
            var sut = new Credit(_accountStore, _messageBus);

            //Act
            var credited = await sut.ExecuteAsync(new CreditActionParams(accountId, 10m));

            //Assert
            Assert.True(credited);
            Assert.Equal(10m, (await _accountReadModelStore.ByIdAsync(accountId)).Get().Balance);
        }

        [Fact]
        public async Task FailureInvalidAmount()
        {
            //Arrange
            AccountId accountId = Guid.NewGuid();
            var account = Account.Open(accountId, 1m).ResultValue;
            await _accountStore.StoreAsync(account);
            await _accountReadModelStore.StoreAsync(new AccountReadModel(accountId, account.Balance));
            var sut = new Credit(_accountStore, _messageBus);

            //Act
            var credited = await sut.ExecuteAsync(new CreditActionParams(accountId, 0m));

            //Assert
            Assert.False(credited);
            Assert.Equal(Errors.InvalidAmount, credited.ErrorValue);
            Assert.Equal(1m, (await _accountReadModelStore.ByIdAsync(accountId)).Get().Balance);
        }

        public void Dispose() =>
            DatabaseSetupDisposer.Dispose(_databaseSetup);
    }
}
