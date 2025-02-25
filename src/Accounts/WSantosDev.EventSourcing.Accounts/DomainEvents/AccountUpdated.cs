using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Accounts.DomainEvents
{
    public record AccountUpdated(AccountId AccountId, Money Balance) : IMessage;
}
