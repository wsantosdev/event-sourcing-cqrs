using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Exchange.Commands
{
    public class Create(ExchangeOrderStore store)
    {
        public async Task<Result<IError>> ExecuteAsync(CreateActionParams @params)
        {
            var order = ExchangeOrder.Create(@params.AccountId, @params.OrderId, @params.Side,
                                             @params.Quantity, @params.Symbol, @params.Price);
            
            await store.StoreAsync(order);
            return true;
        }
    }

    public record CreateActionParams(AccountId AccountId, OrderId OrderId, OrderSide Side, 
                                     Quantity Quantity, Symbol Symbol, Money Price);
}
