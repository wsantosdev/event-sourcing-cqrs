using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Exchange
{
    public interface IOrderStore
    {
        Option<ExchangeOrder> GetById(OrderId orderId);
        
        Result<IError> Store(ExchangeOrder order);
    }
}