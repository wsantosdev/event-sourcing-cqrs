using System.Threading.Tasks;
using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Orders
{
    public interface IOrderStore
    {
        Task<Option<Order>> GetByIdAsync(OrderId orderId);

        Task<Result<IError>> StoreAsync(Order order);
    }
}
