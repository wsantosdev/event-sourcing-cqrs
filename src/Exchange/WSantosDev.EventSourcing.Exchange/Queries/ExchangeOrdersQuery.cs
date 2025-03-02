namespace WSantosDev.EventSourcing.Exchange.Queries
{
    public class ExchangeOrdersQuery(IExchangeOrderReadModelStore readModelStore)
    {
        public async Task<IEnumerable<ExchangeOrderReadModel>> ExecuteAsync() =>
            await readModelStore.GetAllAsync();
    }
}
