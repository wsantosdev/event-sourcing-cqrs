using Microsoft.EntityFrameworkCore;
using NSubstitute;
using WSantosDev.EventSourcing.Accounts.Actions;
using WSantosDev.EventSourcing.Accounts.ExternalEvents;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Accounts.Test
{
    public sealed class OpenActionTest : IDisposable
    {
        private readonly IAccountReadModelStore _accountReadModelStore;
        private readonly IAccountStore _accountStore;
        private readonly IMessageBus _messageBus;

        private readonly EventDbContext _eventDbContext;
        private readonly AccountReadModelDbContext _accountReadModelDbContext;

        public OpenActionTest()
        {
            (_accountStore, _eventDbContext, _accountReadModelStore, _accountReadModelDbContext) 
                = CreateAccountStore();

            _messageBus = Substitute.For<IMessageBus>();
        }

        [Fact]
        public void Success()
        {
            //Arrange
            var action = new OpenAction(_accountReadModelStore, _accountStore, _messageBus);

            //Act
            var opened = action.Execute(new OpenActionParams(Guid.NewGuid(), 1_000_000m));

            //Asset
            Assert.True(opened);
            Assert.NotEmpty(_eventDbContext.Events);
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
            var balance = 1m;
            _accountReadModelDbContext.Accounts.Add(new AccountReadModel(accountId, balance));
            _accountReadModelDbContext.SaveChanges();
            
            var action = new OpenAction(_accountReadModelStore, _accountStore, _messageBus);

            //Act
            var opened = action.Execute(new OpenActionParams(accountId, 1_000_000m));

            //Asset
            Assert.False(opened);
            Assert.Equal(ActionErrors.AccountAlreadyExists, opened.ErrorValue);
            _messageBus.Received(0).Publish(Arg.Any<AccountOpened>());
        }

        #region setup

        public void Dispose()
        {
            _eventDbContext.Events.RemoveRange(_eventDbContext.Events);
            _eventDbContext.SaveChanges();
            _eventDbContext.Dispose();

            _accountReadModelDbContext.Accounts.RemoveRange(_accountReadModelDbContext.Accounts);
            _accountReadModelDbContext.SaveChanges();
            _accountReadModelDbContext.Dispose();
        }

        private static (AccountStore, EventDbContext, AccountReadModelStore, AccountReadModelDbContext) CreateAccountStore()
        {
            var options = SqliteDbContextOptionsBuilderExtensions.UseSqlite(new DbContextOptionsBuilder<EventDbContext>(), "Data Source=./Sqlite/EventStore.sqlite").Options;
            var dbContext = new EventDbContext(options);
            var eventStore = new EventStore(dbContext);

            var readModelOptions = SqliteDbContextOptionsBuilderExtensions.UseSqlite(new DbContextOptionsBuilder<AccountReadModelDbContext>(), "Data Source=./Sqlite/ReadModelStore.sqlite").Options;
            var readModelDbContext = new AccountReadModelDbContext(readModelOptions);
            
            return (new AccountStore(eventStore), dbContext, new AccountReadModelStore(readModelDbContext), readModelDbContext);
        }

        #endregion
    }
}
