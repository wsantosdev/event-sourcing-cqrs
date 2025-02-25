using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Exchange.DomainEvents
{
    public record ExchangeOrderCreated(AccountId AccountId, OrderId OrderId, OrderSide Side, 
                                  Quantity Quantity, Symbol Symbol, Money Price, OrderStatus Status) : IMessage;
}
