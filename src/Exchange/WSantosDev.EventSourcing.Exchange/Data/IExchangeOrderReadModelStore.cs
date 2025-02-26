namespace WSantosDev.EventSourcing.Exchange
{
    public interface IExchangeOrderReadModelStore
    {
        public IEnumerable<ExchangeOrderReadModel> GetAll();

        public void Store(ExchangeOrderReadModel order);
        public void Update(ExchangeOrderReadModel order);
    }
}
