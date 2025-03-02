using System.Collections.Generic;
using System.Threading.Tasks;
using WSantosDev.EventSourcing.Commons;

namespace WSantosDev.EventSourcing.Orders.Queries
{
    public class OrdersByAccountQuery(IOrderReadModelStore readModelStore)
    {
        public async Task<IEnumerable<OrderReadModel>> ExecuteAsync(OrdersByAccountQueryParams queryParams) =>
            await readModelStore.GetByAccountAsync(queryParams.AccountId);
        
    }
    
    public record OrdersByAccountQueryParams(AccountId AccountId);
}
