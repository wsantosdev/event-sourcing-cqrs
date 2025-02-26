using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Commons.Modeling;
using WSantosDev.EventSourcing.Exchange.DomainEvents;

namespace WSantosDev.EventSourcing.Exchange.Actions
{
    public class CreateAction(IExchangeOrderStore store, IMessageBus messageBus)
    {
        public Result<IError> Execute(CreateActionParams @params)
        {
            var order = ExchangeOrder.Create(@params.AccountId, @params.OrderId, @params.Side,
                                             @params.Quantity, @params.Symbol, @params.Price);
            store.Store(order);
            messageBus.Publish(new ExchangeOrderCreated(order.AccountId, order.OrderId, order.Side,
                                                        order.Quantity, order.Symbol, order.Price, order.Status));

            return true;
        }
    }

    public record CreateActionParams(AccountId AccountId, OrderId OrderId, OrderSide Side, 
                                     Quantity Quantity, Symbol Symbol, Money Price);
}
