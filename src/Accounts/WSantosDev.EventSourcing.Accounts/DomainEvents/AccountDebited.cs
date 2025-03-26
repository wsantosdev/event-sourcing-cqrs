using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Accounts
{
    public record AccountDebited(Guid AccountId, decimal Amount) : IMessage;
}
