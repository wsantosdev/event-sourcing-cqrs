using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Commons.Modeling;
using WSantosDev.EventSourcing.Orders.ExternalEvents;

namespace WSantosDev.EventSourcing.Orders.Actions
{
    public class ExecuteAction(IOrderStore store, IMessageBus messageBus)
    {
        public Result<IError> Execute(ExecuteActionParams command)
        {
            var stored = store.GetById(command.OrderId);
            if (stored)
            {
                var order = stored.Get();
                var executed = order.Execute();
                if (executed)
                {
                    store.Store(order);
                    messageBus.Publish(new OrderExecuted(order.AccountId, order.OrderId, order.Side,
                                                         order.Quantity, order.Symbol, order.Price, OrderStatus.Filled));
                }

                return executed;
            }

            return ActionErrors.OrderNotFound;
        }
    }

    public record ExecuteActionParams(OrderId OrderId);
}
