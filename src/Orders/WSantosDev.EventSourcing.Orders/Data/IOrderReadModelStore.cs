using System.Collections.Generic;
using WSantosDev.EventSourcing.Commons;

namespace WSantosDev.EventSourcing.Orders
{
    public interface IOrderReadModelStore
    {
        IEnumerable<OrderReadModel> GetByAccount(AccountId accountId);

        void Store(OrderReadModel order);

        void Update(OrderReadModel order);
    }
}