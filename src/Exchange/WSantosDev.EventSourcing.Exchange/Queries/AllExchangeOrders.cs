namespace WSantosDev.EventSourcing.Exchange.Queries
{
    public class AllExchangeOrders(IExchangeOrderReadModelStore readModelStore)
    {
        public async Task<IEnumerable<ExchangeOrderReadModel>> ExecuteAsync() =>
            await readModelStore.AllAsync();
    }
}
