using System.Collections.Generic;
using System.Threading.Tasks;
using WSantosDev.EventSourcing.Commons;

namespace WSantosDev.EventSourcing.Orders
{
    public interface IOrderReadModelStore
    {
        Task<IEnumerable<OrderReadModel>> GetByAccountAsync(AccountId accountId);

        Task StoreAsync(OrderReadModel order);
    }
}