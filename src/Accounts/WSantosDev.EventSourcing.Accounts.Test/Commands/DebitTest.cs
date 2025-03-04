using WSantosDev.EventSourcing.Accounts.Commands;
using WSantosDev.EventSourcing.Accounts.DomainEvents;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Accounts.Test
{
    public sealed class DebitTest : IDisposable
    {
        private readonly DatabaseSetup _databaseSetup;

        private readonly IAccountReadModelStore _accountReadModelStore;
        private readonly IAccountStore _accountStore;

        private readonly IMessageBus _messageBus;

        public DebitTest()
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
            var account = Account.Open(accountId, 100m).ResultValue;
            await _accountStore.StoreAsync(account);
            await _accountReadModelStore.StoreAsync(new AccountReadModel(accountId, account.Balance));
            var sut = new Debit(_accountStore, _messageBus);

            //Act
            var debited = await sut.ExecuteAsync(new DebitActionParams(accountId, 10m));

            //Assert
            Assert.True(debited);
            Assert.Equal<decimal>(90m, (await _accountStore.GetByIdAsync(accountId)).Get().Balance);
            Assert.Equal(90m, (await _accountReadModelStore.ByIdAsync(accountId)).Get().Balance);
        }

        [Fact]
        public async Task FailureInvalidAmount()
        {
            //Arrange
            AccountId accountId = Guid.NewGuid();
            var account = Account.Open(accountId, 100m).ResultValue;
            await _accountStore.StoreAsync(account);
            await _accountReadModelStore.StoreAsync(new AccountReadModel(accountId, account.Balance));
            var sut = new Debit(_accountStore, _messageBus);

            //Act
            var debited = await sut.ExecuteAsync(new DebitActionParams(accountId, 0m));

            //Assert
            Assert.False(debited);
            Assert.Equal(Errors.InvalidAmount, debited.ErrorValue);
            Assert.Equal<decimal>(100m, (await _accountStore.GetByIdAsync(accountId)).Get().Balance);
            Assert.Equal(100m, (await _accountReadModelStore.ByIdAsync(accountId)).Get().Balance);
        }
                
        [Fact]
        public async Task FailureInsuficientFunds()
        {
            //Arrange
            AccountId accountId = Guid.NewGuid();
            var account = Account.Open(accountId, 100m).ResultValue;
            await _accountStore.StoreAsync(account);
            await _accountReadModelStore.StoreAsync(new AccountReadModel(accountId, account.Balance));
            var sut = new Debit(_accountStore, _messageBus);

            //Act
            var debited = await sut.ExecuteAsync(new DebitActionParams(accountId, 200m));

            //Assert
            Assert.False(debited);
            Assert.Equal(Errors.InsufficientFunds, debited.ErrorValue);
            Assert.Equal<decimal>(100m, (await _accountStore.GetByIdAsync(accountId)).Get().Balance);
            Assert.Equal(100m, (await _accountReadModelStore.ByIdAsync(accountId)).Get().Balance);
        }

        public void Dispose() =>
            DatabaseSetupDisposer.Dispose(_databaseSetup);
    }
}
