namespace WSantosDev.EventSourcing.Exchange
{
    public interface IExchangeOrderReadModelStore
    {
        Task<IEnumerable<ExchangeOrderReadModel>> AllAsync();

        Task StoreAsync(ExchangeOrderReadModel order);
    }
}
