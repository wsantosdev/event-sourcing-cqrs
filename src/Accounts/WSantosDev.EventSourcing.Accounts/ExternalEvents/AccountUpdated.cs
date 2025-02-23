using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Accounts.ExternalEvents
{
    public record AccountUpdated(AccountId AccountId, Money Balance) : IMessage;
}
