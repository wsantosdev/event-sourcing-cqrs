using WSantosDev.EventSourcing.Accounts.Actions;
using WSantosDev.EventSourcing.Accounts.DomainEvents;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Accounts.Test
{
    public sealed class DebitActionTest : IDisposable
    {
        private readonly DatabaseSetup _databaseSetup;

        private readonly IAccountReadModelStore _accountReadModelStore;
        private readonly IAccountStore _accountStore;

        private readonly IMessageBus _messageBus;

        public DebitActionTest()
        {
            _databaseSetup = DatabaseSetupFactory.Create();

            _accountReadModelStore = _databaseSetup.AccountReadModelStore;
            _accountStore = _databaseSetup.AccountStore;

            _messageBus = new InMemoryMessageBus();
            _messageBus.Subscribe(new AccountUpdatedHandler(_accountReadModelStore));
        }

        [Fact]
        public void Success()
        {
            //Arrange
            AccountId accountId = Guid.NewGuid();
            var account = Account.Open(accountId, 100m).ResultValue;
            _accountStore.Store(account);
            _accountReadModelStore.Store(new AccountReadModel(accountId, account.Balance));
            var sut = new DebitAction(_accountStore, _messageBus);

            //Act
            var debited = sut.Execute(new DebitActionParams(accountId, 10m));

            //Assert
            Assert.True(debited);
            Assert.Equal<decimal>(90m, _accountStore.GetById(accountId).Get().Balance);
            Assert.Equal(90m, _accountReadModelStore.GetById(accountId).Get().Balance);
        }

        [Fact]
        public void FailureInvalidAmount()
        {
            //Arrange
            AccountId accountId = Guid.NewGuid();
            var account = Account.Open(accountId, 100m).ResultValue;
            _accountStore.Store(account);
            _accountReadModelStore.Store(new AccountReadModel(accountId, account.Balance));
            var sut = new DebitAction(_accountStore, _messageBus);

            //Act
            var debited = sut.Execute(new DebitActionParams(accountId, 0m));

            //Assert
            Assert.False(debited);
            Assert.Equal(Errors.InvalidAmount, debited.ErrorValue);
            Assert.Equal<decimal>(100m, _accountStore.GetById(accountId).Get().Balance);
            Assert.Equal(100m, _accountReadModelStore.GetById(accountId).Get().Balance);
        }
                
        [Fact]
        public void FailureInsuficientFunds()
        {
            //Arrange
            AccountId accountId = Guid.NewGuid();
            var account = Account.Open(accountId, 100m).ResultValue;
            _accountStore.Store(account);
            _accountReadModelStore.Store(new AccountReadModel(accountId, account.Balance));
            var sut = new DebitAction(_accountStore, _messageBus);

            //Act
            var debited = sut.Execute(new DebitActionParams(accountId, 200m));

            //Assert
            Assert.False(debited);
            Assert.Equal(Errors.InsufficientFunds, debited.ErrorValue);
            Assert.Equal<decimal>(100m, _accountStore.GetById(accountId).Get().Balance);
            Assert.Equal(100m, _accountReadModelStore.GetById(accountId).Get().Balance);
        }

        public void Dispose() =>
            DatabaseSetupDisposer.Dispose(_databaseSetup);
    }
}
