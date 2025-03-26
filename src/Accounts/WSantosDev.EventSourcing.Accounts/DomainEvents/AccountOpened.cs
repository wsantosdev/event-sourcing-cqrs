using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Accounts
{
    public record AccountOpened(Guid AccountId, decimal InitialDeposit) : IMessage;
}
