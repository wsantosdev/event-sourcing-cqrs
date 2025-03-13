using Microsoft.EntityFrameworkCore;
using WSantosDev.EventSourcing.Commons;

namespace WSantosDev.EventSourcing.Orders
{
    public class OrderViewStore(OrderViewDbContext dbContext)
    {
        public async Task<IEnumerable<OrderView>> GetByAccountAsync(AccountId accountId) =>
            await dbContext.ByAccountIdAsync(accountId);

        public async Task StoreAsync(OrderView order)
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
