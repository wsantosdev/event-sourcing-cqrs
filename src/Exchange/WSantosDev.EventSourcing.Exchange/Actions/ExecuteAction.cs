using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Commons.Modeling;
using WSantosDev.EventSourcing.Exchange.DomainEvents;

namespace WSantosDev.EventSourcing.Exchange.Actions
{
    public class ExecuteAction(IExchangeOrderStore store, IMessageBus messageBus)
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
                    messageBus.Publish(new ExchangeOrderExecuted(order.AccountId, order.OrderId, order.Side,
                                                            order.Quantity, order.Symbol, order.Price, order.Status));

                    return true;
                }

                return Result<IError>.Error(executed.ErrorValue);
            }

            return ActionErrors.OrderNotFound;
        }
    }

    public record ExecuteActionParams(OrderId OrderId);
}
