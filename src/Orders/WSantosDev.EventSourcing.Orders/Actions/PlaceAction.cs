using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Commons.Modeling;
using WSantosDev.EventSourcing.Orders.ExternalEvents;

namespace WSantosDev.EventSourcing.Orders.Actions
{
    public class PlaceAction(IOrderStore store, IMessageBus messageBus)
    {
        public Result<Order, IError> Execute(PlaceActionParams command)
        {
            var created = Order.New(command.AccountId, command.OrderId, command.Side, command.Quantity, command.Symbol, command.Price);
            if (created)
            {
                var order = created.ResultValue;
                store.Store(order);
                messageBus.Publish(new OrderPlaced(order.AccountId, order.OrderId, order.Side,
                                                   order.Quantity, order.Symbol, order.Price,
                                                   order.Status));
            }

            return created;
        }
    }

    public record PlaceActionParams(AccountId AccountId, OrderId OrderId, OrderSide Side, 
                                    Quantity Quantity, Symbol Symbol, Money Price);
}
