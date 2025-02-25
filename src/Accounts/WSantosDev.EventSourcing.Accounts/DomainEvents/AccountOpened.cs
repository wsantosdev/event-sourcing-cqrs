using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Accounts.DomainEvents
{
    public record AccountOpened(AccountId AccountId, Money InitialDeposit) : IMessage;
}
