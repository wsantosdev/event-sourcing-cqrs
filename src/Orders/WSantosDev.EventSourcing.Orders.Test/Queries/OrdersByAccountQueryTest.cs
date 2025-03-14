using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Orders.Queries;

namespace WSantosDev.EventSourcing.Orders.Test
{
    public sealed class OrdersByAccountQueryTest : IDisposable
    {
        private readonly Database _database;

        public OrdersByAccountQueryTest()
        {
            _database = DatabaseFactory.Create();
        }

        [Fact]
        public async Task Success()
        {
            //Arrange
            var accountId = Guid.NewGuid();
            var order = Order.New(accountId, Guid.NewGuid(), OrderSide.Buy, 10, "CSCO", 10m);
            
            await _database.ViewStore.StoreAsync(OrderView.CreateFrom(order));
            var sut = new OrdersByAccount(_database.ViewDbContext);

            //Act
            var orders = await sut.ExecuteAsync(new OrdersByAccountParams(accountId));

            //Assert
            Assert.NotEmpty(orders);
            Assert.Equal(accountId, orders.First().AccountId);
        }

        [Fact]
        public async Task SuccessButNotFound()
        {
            //Arrange
            var sut = new OrdersByAccount(_database.ViewDbContext);

            //Act
            var orders = await sut.ExecuteAsync(new OrdersByAccountParams(Guid.NewGuid()));

            //Assert
            Assert.Empty(orders);
        }

        public void Dispose() =>
            DatabaseDisposer.Dispose(_database);
    }
}
