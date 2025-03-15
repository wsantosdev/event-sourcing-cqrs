using Microsoft.EntityFrameworkCore;
using Moonad;

namespace WSantosDev.EventSourcing.Exchange
{
    public class ExchangeOrderViewStore(ExchangeOrderViewDbContext dbContext)
    {
        public async Task<IEnumerable<ExchangeOrderView>> ListAsync(CancellationToken cancellationToken = default) =>
            await dbContext.ListAsync(cancellationToken);

        public async Task StoreAsync(ExchangeOrderView view)
        {
            var stored = await dbContext.ByOrderIdAsync(view.OrderId);
            if (stored)
            {
                var storedView = stored.Get();
                storedView.UpdateFrom(view);
                dbContext.Entry(storedView).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return;
            }

            dbContext.Add(view);
            await dbContext.SaveChangesAsync();
        }
    }
}
