using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Positions.ExternalEvents
{
    public record PositionOpened(AccountId AccountId, Symbol Symbol, Quantity Available) : IMessage;
}
