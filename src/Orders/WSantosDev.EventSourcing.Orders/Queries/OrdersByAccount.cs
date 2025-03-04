using System.Collections.Generic;
using System.Threading.Tasks;
using WSantosDev.EventSourcing.Commons;

namespace WSantosDev.EventSourcing.Orders.Queries
{
    public class OrdersByAccount(IOrderReadModelStore readModelStore)
    {
        public async Task<IEnumerable<OrderReadModel>> ExecuteAsync(OrdersByAccountParams queryParams) =>
            await readModelStore.GetByAccountAsync(queryParams.AccountId);
        
    }
    
    public record OrdersByAccountParams(AccountId AccountId);
}
