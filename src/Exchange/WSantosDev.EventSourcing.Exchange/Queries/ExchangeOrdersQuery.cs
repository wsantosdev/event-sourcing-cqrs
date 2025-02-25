namespace WSantosDev.EventSourcing.Exchange.Queries
{
    public class ExchangeOrdersQuery(IExchangeOrderReadModelStore readModelStore)
    {
        public IEnumerable<OrderReadModel> Execute() =>
            readModelStore.GetAll();
    }
}
