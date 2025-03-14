using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Commons.Modeling;
using WSantosDev.EventSourcing.Orders.Commands;

namespace WSantosDev.EventSourcing.Orders.Test
{
    public sealed class PlaceTest : IDisposable
    {
        private readonly Database _database;
        private readonly InMemoryMessageBus _messageBus;

        public PlaceTest()
        {
            _database = DatabaseFactory.Create();
            _messageBus = new InMemoryMessageBus();
        }

        public void Dispose() =>
            DatabaseDisposer.Dispose(_database);

        [Fact]
        public async Task Success()
        {
            //Arrange
            var accountId = Guid.NewGuid();
            var orderId = Guid.NewGuid();
            var sut = new Place(_database.Store, _messageBus);

            //Act
            var placed = await sut.ExecuteAsync(new PlaceParams(accountId, orderId, OrderSide.Buy, 100, "CSCO", 10m));

            //Assert
            Assert.True(placed);
            Assert.True(await _database.Store.ByIdAsync(orderId));
            Assert.NotEmpty(await _database.ViewStore.GetByAccountAsync(accountId));
        }

        [Theory]
        [MemberData(nameof(FailureTestData))]
        public async Task Failure(AccountId accountId, OrderId orderId, OrderSide orderSide, Quantity quantity, Symbol symbol, Money price, IError expectedError)
        {
            //Arrange
            var sut = new Place(_database.Store, _messageBus);

            //Act
            var placed = await sut.ExecuteAsync(new PlaceParams(accountId, orderId, orderSide, quantity, symbol, price));

            //Assert
            Assert.False(placed);
            Assert.Equal(expectedError, placed.ErrorValue);
            Assert.False(await _database.Store.ByIdAsync(orderId));
            Assert.Empty(await _database.ViewStore.GetByAccountAsync(accountId));
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
    }
}
