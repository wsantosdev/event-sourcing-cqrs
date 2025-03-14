using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Commons.Modeling;
using WSantosDev.EventSourcing.Exchange.DomainEvents;

namespace WSantosDev.EventSourcing.Exchange.Commands
{
    public class Execute(ExchangeOrderStore store, IMessageBus messageBus)
    {
        public async Task<Result<IError>> ExecuteAsync(ExecuteActionParams command)
        {
            var stored = await store.ByIdAsync(command.OrderId);
            if (stored)
            {
                var exchangeOrder = stored.Get();
                var executed = exchangeOrder.Execute();
                if (executed)
                {
                    await store.StoreAsync(exchangeOrder);
                    messageBus.Publish(new ExchangeOrderExecuted(exchangeOrder.AccountId, exchangeOrder.OrderId, exchangeOrder.Side, 
                                                                 exchangeOrder.Quantity, exchangeOrder.Symbol, exchangeOrder.Price, exchangeOrder.Status));
                }

                return executed;
            }

            return CommandsErrors.OrderNotFound;
        }
    }

    public record ExecuteActionParams(OrderId OrderId);
}
