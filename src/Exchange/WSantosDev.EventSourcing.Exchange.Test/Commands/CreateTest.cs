using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Exchange.Commands;

namespace WSantosDev.EventSourcing.Exchange.Test
{
    public sealed class CreateTest : IDisposable
    {
        private readonly Database _database;

        public CreateTest()
        {
            _database = DatabaseFactory.Create();
        }

        [Fact]
        public async Task Success() 
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var sut = new Create(_database.Store);

            //Act
            var created = await sut.ExecuteAsync(new CreateActionParams(Guid.NewGuid(), orderId, OrderSide.Buy, 10, "NVDA", 10m));

            //Assert
            Assert.True(created);
            Assert.True(await _database.Store.ByIdAsync(orderId));
            Assert.NotEmpty((await _database.ViewStore.AllAsync()).Select(o => o.OrderId == orderId));
        }

        public void Dispose() =>
            DatabaseDisposer.Dispose(_database);
    }
}
