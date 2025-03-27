using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Exchange
{
    public sealed partial class ExchangeOrder
    {
        private record struct OrderCreated(int Id, AccountId AccountId, OrderId OrderId, string Side, int Quantity,
                                           string Symbol, decimal Price, string OrderStatus) : IEvent;
        private record struct OrderExecuted(int Id, AccountId AccountId, OrderId OrderId, string Side, int Quantity,
                                           string Symbol, decimal Price, string OrderStatus) : IEvent;
    }
}
