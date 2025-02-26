using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Exchange.Queries;

namespace WSantosDev.EventSourcing.Exchange.Test.Queries
{
    public sealed class ExchangeOrdersQueryTest : IDisposable
    {
        private readonly DatabaseSetup _databaseSetup;
        private readonly ExchangeOrderReadModelStore _readModelStore;

        public ExchangeOrdersQueryTest()
        {
            _databaseSetup = DatabaseSetupFactory.Create();
            _readModelStore = _databaseSetup.ReadModelStore;
        }

        [Fact]
        public void Success() 
        {
            //Arrange
            _readModelStore.Store(new ExchangeOrderReadModel(Guid.NewGuid(), Guid.NewGuid(), OrderSide.Sell, 100, 
                                                             "MNST", 10m, OrderStatus.Filled));
            var sut = new ExchangeOrdersQuery(_readModelStore);

            //Act
            var orders = sut.Execute();

            //Assert
            Assert.NotEmpty(orders);
        }

        [Fact]
        public void SuccessButNotFound()
        {
            //Arrange
            var sut = new ExchangeOrdersQuery(_readModelStore);

            //Act
            var orders = sut.Execute();

            //Assert
            Assert.Empty(orders);
        }

        public void Dispose() =>
            DatabaseSetupDisposer.Dispose(_databaseSetup);
    }
}
