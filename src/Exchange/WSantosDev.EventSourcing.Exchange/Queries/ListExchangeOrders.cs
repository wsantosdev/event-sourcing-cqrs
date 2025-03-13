using Microsoft.EntityFrameworkCore;

namespace WSantosDev.EventSourcing.Exchange.Queries
{
    public class ListExchangeOrders(ExchangeOrderViewDbContext dbContext)
    {
        public async Task<IEnumerable<ExchangeOrderView>> ExecuteAsync() =>
            await dbContext.ExchangeOrders.ToListAsync();
    }
}
