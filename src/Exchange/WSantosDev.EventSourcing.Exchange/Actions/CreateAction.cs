using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Exchange.DomainEvents;

namespace WSantosDev.EventSourcing.Exchange.Actions
{
    public class CreateAction(IExchangeOrderStore store, IMessageBus messageBus)
    {
        public void Execute(CreateActionParams command)
        {
            var order = ExchangeOrder.Create(command.AccountId, command.OrderId, command.Side,
                                             command.Quantity, command.Symbol, command.Price);
            store.Store(order);
            messageBus.Publish(new ExchangeCreated(order.AccountId, order.OrderId, order.Side,
                                                   order.Quantity, order.Symbol, order.Price, order.Status));
        }
    }

    public record CreateActionParams(AccountId AccountId, OrderId OrderId, OrderSide Side, 
                                     Quantity Quantity, Symbol Symbol, Money Price);
}
