using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Commons.Modeling;
using WSantosDev.EventSourcing.Orders.Actions;
using WSantosDev.EventSourcing.Orders.DomainEvents;

namespace WSantosDev.EventSourcing.Orders.Test
{
    public sealed class PlaceActionTest : IDisposable
    {
        private readonly DatabaseSetup _databaseSetup;

        private readonly OrderReadModelStore _readModelStore;
        private readonly OrderStore _store;

        private readonly InMemoryMessageBus _messageBus;

        public PlaceActionTest()
        {
            _databaseSetup = DatabaseSetupFactory.Create();

            _readModelStore = _databaseSetup.ReadModelStore;
            _store = _databaseSetup.Store;

            _messageBus = new InMemoryMessageBus();
            _messageBus.Subscribe(new OrderPlacedHandler(_readModelStore));
        }

        [Fact]
        public void Success()
        {
            //Arrange
            var accountId = Guid.NewGuid();
            var orderId = Guid.NewGuid();
            var sut = new PlaceAction(_store, _messageBus);

            //Act
            var placed = sut.Execute(new PlaceActionParams(accountId, orderId, OrderSide.Buy, 100, "CSCO", 10m));

            //Assert
            Assert.True(placed);
            Assert.True(_store.GetById(orderId));
            Assert.NotEmpty(_readModelStore.GetByAccount(accountId));
        }

        [Theory]
        [MemberData(nameof(FailureTestData))]
        public void Failure(AccountId accountId, OrderId orderId, OrderSide orderSide, Quantity quantity, Symbol symbol, Money price, IError expectedError)
        {
            //Arrange
            var sut = new PlaceAction(_store, _messageBus);

            //Act
            var placed = sut.Execute(new PlaceActionParams(accountId, orderId, orderSide, quantity, symbol, price));

            //Assert
            Assert.False(placed);
            Assert.Equal(expectedError, placed.ErrorValue);
            Assert.False(_store.GetById(orderId));
            Assert.Empty(_readModelStore.GetByAccount(accountId));
        }

        public static IEnumerable<object[]> FailureTestData =>
        [
            [AccountId.Empty, Guid.NewGuid(), OrderSide.Buy, 100, "MSFT", 10m, Errors.EmptyAccountId],
            [Guid.NewGuid(), OrderId.Empty, OrderSide.Buy, 100, "MSFT", 10m, Errors.EmptyOrderId],
            [Guid.NewGuid(), Guid.NewGuid(), "undefined", 100, "MSFT", 10m, Errors.InvalidSide],
            [Guid.NewGuid(), Guid.NewGuid(), OrderSide.Buy, 0, "MSFT", 10m, Errors.InvalidQuantity],
            [Guid.NewGuid(), Guid.NewGuid(), OrderSide.Buy, 100, "", 10m, Errors.InvalidSymbol],
            [Guid.NewGuid(), Guid.NewGuid(), OrderSide.Buy, 100, "MSFT", 0m, Errors.InvalidPrice]
        ];

        public void Dispose() =>
            DatabaseSetupDisposer.Dispose(_databaseSetup);
    }
}
