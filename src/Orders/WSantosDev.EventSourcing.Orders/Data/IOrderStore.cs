using System.Threading.Tasks;
using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Orders
{
    public interface IOrderStore
    {
        Option<Order> GetById(OrderId orderId);

        Task<Result<IError>> StoreAsync(Order order);
    }
}
