namespace WSantosDev.EventSourcing.Exchange.Queries
{
    public class ExchangeOrdersQuery(IExchangeOrderReadModelStore readModelStore)
    {
        public IEnumerable<ExchangeOrderReadModel> Execute() =>
            readModelStore.GetAll();
    }
}
