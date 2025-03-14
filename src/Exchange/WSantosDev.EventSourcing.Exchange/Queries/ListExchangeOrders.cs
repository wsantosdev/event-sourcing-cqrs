namespace WSantosDev.EventSourcing.Exchange.Queries
{
    public class ListExchangeOrders(ExchangeOrderViewDbContext dbContext)
    {
        public async Task<IEnumerable<ExchangeOrderView>> ExecuteAsync(CancellationToken cancellationToken = default) =>
            await dbContext.ListAsync(cancellationToken);
    }
}
