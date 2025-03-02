namespace WSantosDev.EventSourcing.Exchange
{
    public interface IExchangeOrderReadModelStore
    {
        Task<IEnumerable<ExchangeOrderReadModel>> GetAllAsync();

        Task StoreAsync(ExchangeOrderReadModel order);
    }
}
