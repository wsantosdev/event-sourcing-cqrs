using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Exchange.Queries;

namespace WSantosDev.EventSourcing.Exchange.Test.Queries
{
    public sealed class ListExchangeOrdersTest : IDisposable
    {
        private readonly Database _database;
        
        public ListExchangeOrdersTest()
        {
            _database = DatabaseFactory.Create();
        }

        [Fact]
        public async Task Success() 
        {
            //Arrange
            await _database.ViewStore.StoreAsync(new ExchangeOrderView(Guid.NewGuid(), Guid.NewGuid(), OrderSide.Sell, 100, 
                                                                       "MNST", 10m, OrderStatus.Filled));
            var sut = new ListExchangeOrders(_database.ViewDbContext);

            //Act
            var orders = await sut.ExecuteAsync();

            //Assert
            Assert.NotEmpty(orders);
        }

        [Fact]
        public async Task SuccessButNotFound()
        {
            //Arrange
            var sut = new ListExchangeOrders(_database.ViewDbContext);

            //Act
            var orders = await sut.ExecuteAsync();

            //Assert
            Assert.Empty(orders);
        }

        public void Dispose() =>
            DatabaseDisposer.Dispose(_database);
    }
}
