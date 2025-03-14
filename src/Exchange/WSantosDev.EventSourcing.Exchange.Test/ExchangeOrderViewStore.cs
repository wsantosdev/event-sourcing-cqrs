using Microsoft.EntityFrameworkCore;
using Moonad;

namespace WSantosDev.EventSourcing.Exchange
{
    public class ExchangeOrderViewStore(ExchangeOrderViewDbContext dbContext)
    {
        public async Task<IEnumerable<ExchangeOrderView>> ListAsync(CancellationToken cancellationToken = default) =>
            await dbContext.ListAsync(cancellationToken);

        public async Task StoreAsync(ExchangeOrderView order)
        {
            var stored = await dbContext.ByOrderIdAsync(order.OrderId);
            if (stored)
            {
                dbContext.Entry(stored.Get()).State = EntityState.Detached;
                dbContext.Entry(order).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return;
            }

            dbContext.Add(order);
            await dbContext.SaveChangesAsync();
        }
    }
}
