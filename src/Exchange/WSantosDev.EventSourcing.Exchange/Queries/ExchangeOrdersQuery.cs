namespace WSantosDev.EventSourcing.Exchange.Queries
{
    public class ExchangeOrdersQuery(IOrderReadModelStore readModelStore)
    {
        public IEnumerable<OrderReadModel> Execute() =>
            readModelStore.GetAll();
    }
}
