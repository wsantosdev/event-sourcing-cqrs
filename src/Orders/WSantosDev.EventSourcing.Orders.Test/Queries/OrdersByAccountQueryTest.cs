using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Orders.Queries;

namespace WSantosDev.EventSourcing.Orders.Test
{
    public sealed class OrdersByAccountQueryTest : IDisposable
    {
        private readonly DatabaseSetup _databaseSetup;

        private readonly OrderReadModelStore _readModelStore;

        public OrdersByAccountQueryTest()
        {
            _databaseSetup = DatabaseSetupFactory.Create();
            
            _readModelStore = _databaseSetup.ReadModelStore;
        }

        [Fact]
        public void Success()
        {
            //Arrange
            var accountId = Guid.NewGuid();
            _readModelStore.Store(new OrderReadModel(accountId, Guid.NewGuid(), OrderSide.Buy, 10, "CSCO", 10m, OrderStatus.Filled));
            var sut = new OrdersByAccountQuery(_readModelStore);

            //Act
            var orders = sut.Execute(new OrdersByAccountQueryParams(accountId));

            //Assert
            Assert.NotEmpty(orders);
            Assert.Equal(accountId, orders.First().AccountId);
        }

        [Fact]
        public void SuccessButNotFound()
        {
            //Arrange
            var sut = new OrdersByAccountQuery(_readModelStore);

            //Act
            var orders = sut.Execute(new OrdersByAccountQueryParams(Guid.NewGuid()));

            //Assert
            Assert.Empty(orders);
        }

        public void Dispose() =>
            DatabaseSetupDisposer.Dispose(_databaseSetup);
    }
}
