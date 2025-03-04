using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Exchange.Queries;

namespace WSantosDev.EventSourcing.Exchange.Test.Queries
{
    public sealed class AllExchangeOrdersTest : IDisposable
    {
        private readonly DatabaseSetup _databaseSetup;
        private readonly ExchangeOrderReadModelStore _readModelStore;

        public AllExchangeOrdersTest()
        {
            _databaseSetup = DatabaseSetupFactory.Create();
            _readModelStore = _databaseSetup.ReadModelStore;
        }

        [Fact]
        public async Task Success() 
        {
            //Arrange
            await _readModelStore.StoreAsync(new ExchangeOrderReadModel(Guid.NewGuid(), Guid.NewGuid(), OrderSide.Sell, 100, 
                                                             "MNST", 10m, OrderStatus.Filled));
            var sut = new AllExchangeOrders(_readModelStore);

            //Act
            var orders = await sut.ExecuteAsync();

            //Assert
            Assert.NotEmpty(orders);
        }

        [Fact]
        public async Task SuccessButNotFound()
        {
            //Arrange
            var sut = new AllExchangeOrders(_readModelStore);

            //Act
            var orders = await sut.ExecuteAsync();

            //Assert
            Assert.Empty(orders);
        }

        public void Dispose() =>
            DatabaseSetupDisposer.Dispose(_databaseSetup);
    }
}
