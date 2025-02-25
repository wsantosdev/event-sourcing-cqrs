using WSantosDev.EventSourcing.Accounts.Actions;
using WSantosDev.EventSourcing.Accounts.DomainEvents;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Accounts.Test
{
    public sealed class OpenActionTest : IDisposable
    {
        private readonly DatabaseSetup _databaseSetup;

        private readonly IAccountReadModelStore _accountReadModelStore;
        private readonly IAccountStore _accountStore;
        
        private readonly IMessageBus _messageBus;

        public OpenActionTest()
        {
            _databaseSetup = DatabaseSetupFactory.Create();

            _accountReadModelStore = _databaseSetup.AccountReadModelStore;
            _accountStore = _databaseSetup.AccountStore;

            _messageBus = new InMemoryMessageBus();
            _messageBus.Subscribe(new AccountOpenedHandler(_accountReadModelStore));
        }

        [Fact]
        public void Success()
        {
            //Arrange
            AccountId accountId = Guid.NewGuid();
            var sut = new OpenAction(_accountReadModelStore, _accountStore, _messageBus);

            //Act
            var opened = sut.Execute(new OpenActionParams(accountId, 1_000_000m));

            //Assert
            Assert.True(opened);
            Assert.True(_accountStore.GetById(accountId));
            Assert.True(_accountReadModelStore.GetById(accountId));
        }

        [Fact]
        public void SuccessWithNoInitialDeposit()
        {
            //Arrange
            AccountId accountId = Guid.NewGuid();
            var sut = new OpenAction(_accountReadModelStore, _accountStore, _messageBus);

            //Act
            var opened = sut.Execute(new OpenActionParams(accountId, 0m));
            
            //Assert
            Assert.True(opened);
            var account = _accountStore.GetById(accountId);
            Assert.True(account);
            Assert.Equal<decimal>(0m, account.Get().Balance);
            var accountReadModel = _accountReadModelStore.GetById(accountId);
            Assert.True(accountReadModel);
            Assert.Equal(0m, accountReadModel.Get().Balance);
        }

        [Fact]
        public void FailureEmptyAccountId()
        {
            //Arrange
            var accountId = AccountId.Empty;
            var sut = new OpenAction(_accountReadModelStore, _accountStore, _messageBus);

            //Act
            var opened = sut.Execute(new OpenActionParams(accountId, 1_000_000m));

            //Assert
            Assert.False(opened);
            Assert.Equal(Errors.EmptyAccountId, opened.ErrorValue);
            Assert.False(_accountReadModelStore.GetById(accountId));
        }

        [Fact]
        public void FailureAccountAlreadyExists()
        {
            //Arrange
            AccountId accountId = Guid.NewGuid();
            _accountReadModelStore.Store(new AccountReadModel(accountId, 1m));
            
            var sut = new OpenAction(_accountReadModelStore, _accountStore, _messageBus);

            //Act
            var opened = sut.Execute(new OpenActionParams(accountId, 1_000_000m));

            //Assert
            Assert.False(opened);
            Assert.Equal(ActionErrors.AccountAlreadyExists, opened.ErrorValue);
            Assert.True(_accountReadModelStore.GetById(accountId));
        }

        public void Dispose() =>
            DatabaseSetupDisposer.Dispose(_databaseSetup);
    }
}
