using System.Collections.Generic;
using System.Threading.Tasks;
using WSantosDev.EventSourcing.Commons;

namespace WSantosDev.EventSourcing.Orders.Queries
{
    public class OrdersByAccount(OrderViewDbContext viewDbContext)
    {
        public async Task<IEnumerable<OrderView>> ExecuteAsync(OrdersByAccountParams queryParams) =>
            await viewDbContext.ByAccountIdAsync(queryParams.AccountId);
        
    }
    
    public record OrdersByAccountParams(AccountId AccountId);
}
