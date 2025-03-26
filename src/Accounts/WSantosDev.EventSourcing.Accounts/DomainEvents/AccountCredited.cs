using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Accounts
{
    public record AccountCredited(Guid AccountId, decimal Amount) : IMessage;
}
