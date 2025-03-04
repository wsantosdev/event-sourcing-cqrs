using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Positions.DomainEvents
{
    public record Withdrawn(AccountId AccountId, Symbol Symbol, Quantity Available) : IMessage;
}
