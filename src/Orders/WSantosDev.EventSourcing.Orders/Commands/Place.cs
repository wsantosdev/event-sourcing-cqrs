using System.Threading.Tasks;
using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Commons.Modeling;
using WSantosDev.EventSourcing.Orders.DomainEvents;

namespace WSantosDev.EventSourcing.Orders.Commands
{
    public class Place(OrderStore store, IMessageBus messageBus)
    {
        public async Task<Result<Order, IError>> ExecuteAsync(PlaceParams command)
        {
            var created = Order.New(command.AccountId, command.OrderId, command.Side, 
                                    command.Quantity, command.Symbol, command.Price);

            if (created)
            {
                var order = created.ResultValue;
                await store.StoreAsync(order);
                messageBus.Publish(new OrderPlaced(order.AccountId, order.OrderId, order.Side, 
                                                   order.Quantity, order.Symbol, order.Price, order.Status));
            }

            return created;
        }
    }

    public record PlaceParams(AccountId AccountId, OrderId OrderId, OrderSide Side, 
                                    Quantity Quantity, Symbol Symbol, Money Price);
}
