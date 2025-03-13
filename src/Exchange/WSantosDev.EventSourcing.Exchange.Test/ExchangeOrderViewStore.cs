using Microsoft.EntityFrameworkCore;
using Moonad;

namespace WSantosDev.EventSourcing.Exchange
{
    public class ExchangeOrderViewStore(ExchangeOrderViewDbContext context)
    {
        public async Task<IEnumerable<ExchangeOrderView>> AllAsync() =>
            await context.ExchangeOrders.ToListAsync();

        public async Task StoreAsync(ExchangeOrderView order)
        {
            var stored = (await context.ExchangeOrders.FirstOrDefaultAsync(o => o.OrderId == order.OrderId)).ToOption();
            if (stored)
            {
                context.ExchangeOrders.Entry(stored.Get()).State = EntityState.Detached;
                context.ExchangeOrders.Entry(order).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return;
            }

            context.Add(order);
            await context.SaveChangesAsync();
        }
    }
}
