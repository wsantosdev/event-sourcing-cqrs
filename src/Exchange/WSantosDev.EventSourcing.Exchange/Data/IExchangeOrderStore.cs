using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Exchange
{
    public interface IExchangeOrderStore
    {
        Task<Option<ExchangeOrder>> GetByIdAsync(OrderId orderId);
        
        Task<Result<IError>> StoreAsync(ExchangeOrder order);
    }
}