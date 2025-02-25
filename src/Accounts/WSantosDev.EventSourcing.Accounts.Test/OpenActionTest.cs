using NSubstitute;
using WSantosDev.EventSourcing.Accounts.Actions;
using WSantosDev.EventSourcing.Accounts.ExternalEvents;
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

            _messageBus = Substitute.For<IMessageBus>();
        }

        [Fact]
        public void Success()
        {
            //Arrange
            AccountId accountId = Guid.NewGuid();
            var action = new OpenAction(_accountReadModelStore, _accountStore, _messageBus);

            //Act
            var opened = action.Execute(new OpenActionParams(accountId, 1_000_000m));

            //Asset
            Assert.True(opened);
            Assert.True(_accountStore.GetById(accountId));
            _messageBus.Received(1).Publish(Arg.Any<AccountOpened>());
        }

        [Fact]
        public void FailureEmptyAccountId()
        {
            //Arrange
            var accountId = AccountId.Empty;
            var action = new OpenAction(_accountReadModelStore, _accountStore, _messageBus);

            //Act
            var opened = action.Execute(new OpenActionParams(accountId, 1_000_000m));

            //Asset
            Assert.False(opened);
            Assert.Equal(Errors.EmptyAccountId, opened.ErrorValue);
            _messageBus.Received(0).Publish(Arg.Any<AccountOpened>());
        }

        [Fact]
        public void FailureAccountAlreadyExists()
        {
            //Arrange
            var accountId = Guid.NewGuid();
            _accountReadModelStore.Store(new AccountReadModel(Guid.NewGuid(), 1m));
            
            var action = new OpenAction(_accountReadModelStore, _accountStore, _messageBus);

            //Act
            var opened = action.Execute(new OpenActionParams(accountId, 1_000_000m));

            //Asset
            Assert.False(opened);
            Assert.Equal(ActionErrors.AccountAlreadyExists, opened.ErrorValue);
            _messageBus.Received(0).Publish(Arg.Any<AccountOpened>());
        }

        #region dispose

        public void Dispose()
        {
            DatabaseSetupDisposer.Dispose(_databaseSetup);
        }

        #endregion
    }
}
