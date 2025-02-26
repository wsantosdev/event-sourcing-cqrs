using WSantosDev.EventSourcing.Accounts.Actions;
using WSantosDev.EventSourcing.Accounts.DomainEvents;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Accounts.Test
{
    public sealed class OpenActionTest : IDisposable
    {
        private readonly DatabaseSetup _databaseSetup;

        private readonly AccountReadModelStore _readModelStore;
        private readonly AccountStore _store;
        
        private readonly InMemoryMessageBus _messageBus;

        public OpenActionTest()
        {
            _databaseSetup = DatabaseSetupFactory.Create();

            _readModelStore = _databaseSetup.ReadModelStore;
            _store = _databaseSetup.Store;

            _messageBus = new InMemoryMessageBus();
            _messageBus.Subscribe(new AccountOpenedHandler(_readModelStore));
        }

        [Fact]
        public void Success()
        {
            //Arrange
            AccountId accountId = Guid.NewGuid();
            var sut = new OpenAction(_store, _messageBus);

            //Act
            var opened = sut.Execute(new OpenActionParams(accountId, 1_000_000m));

            //Assert
            Assert.True(opened);
            Assert.True(_store.GetById(accountId));
            Assert.True(_readModelStore.GetById(accountId));
        }

        [Fact]
        public void SuccessWithNoInitialDeposit()
        {
            //Arrange
            AccountId accountId = Guid.NewGuid();
            var sut = new OpenAction(_store, _messageBus);

            //Act
            var opened = sut.Execute(new OpenActionParams(accountId, 0m));
            
            //Assert
            Assert.True(opened);
            var account = _store.GetById(accountId);
            Assert.True(account);
            Assert.Equal<decimal>(0m, account.Get().Balance);
            var accountReadModel = _readModelStore.GetById(accountId);
            Assert.True(accountReadModel);
            Assert.Equal(0m, accountReadModel.Get().Balance);
        }

        [Fact]
        public void FailureEmptyAccountId()
        {
            //Arrange
            var accountId = AccountId.Empty;
            var sut = new OpenAction(_store, _messageBus);

            //Act
            var opened = sut.Execute(new OpenActionParams(accountId, 1_000_000m));

            //Assert
            Assert.False(opened);
            Assert.Equal(Errors.EmptyAccountId, opened.ErrorValue);
            Assert.False(_readModelStore.GetById(accountId));
        }

        [Fact]
        public void FailureAccountAlreadyExists()
        {
            //Arrange
            AccountId accountId = Guid.NewGuid();
            _readModelStore.Store(new AccountReadModel(accountId, 1m));
            
            var sut = new OpenAction(_store, _messageBus);

            //Act
            var opened = sut.Execute(new OpenActionParams(accountId, 1_000_000m));

            //Assert
            Assert.False(opened);
            Assert.Equal(ActionErrors.AccountAlreadyExists, opened.ErrorValue);
            Assert.True(_readModelStore.GetById(accountId));
        }

        public void Dispose() =>
            DatabaseSetupDisposer.Dispose(_databaseSetup);
    }
}
