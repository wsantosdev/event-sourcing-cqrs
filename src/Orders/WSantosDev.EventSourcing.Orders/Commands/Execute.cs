using System.Threading.Tasks;
using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Commons.Modeling;
using WSantosDev.EventSourcing.Orders.DomainEvents;

namespace WSantosDev.EventSourcing.Orders.Actions
{
    public class Execute(OrderStore store, IMessageBus messageBus)
    {
        public async Task<Result<IError>> ExecuteAsync(ExecuteActionParams command)
        {
            var stored = await store.ByIdAsync(command.OrderId);
            if (stored)
            {
                var order = stored.Get();
                var executed = order.Execute();
                if (executed)
                {
                    await store.StoreAsync(order);
                    messageBus.Publish(new OrderExecuted(order.AccountId, order.OrderId, order.Side, 
                                                         order.Quantity, order.Symbol, order.Price, order.Status));
                }

                return executed;
            }

            return CommandErrors.OrderNotFound;
        }
    }

    public record ExecuteActionParams(OrderId OrderId);
}
