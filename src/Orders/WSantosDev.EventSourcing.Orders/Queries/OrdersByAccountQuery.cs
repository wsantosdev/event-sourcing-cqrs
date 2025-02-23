using System.Collections.Generic;
using WSantosDev.EventSourcing.Commons;

namespace WSantosDev.EventSourcing.Orders.Queries
{
    public class OrdersByAccountQuery(IOrderReadModelStore readModelStore)
    {
        public IEnumerable<OrderReadModel> Execute(OrdersByAccountQueryParams queryParams) =>
            readModelStore.GetByAccount(queryParams.AccountId);
        
    }
    
    public record OrdersByAccountQueryParams(AccountId AccountId);
}
