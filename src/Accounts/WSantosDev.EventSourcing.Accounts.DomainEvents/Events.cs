using System;
using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Accounts
{
    public record AccountOpened(Guid AccountId, decimal InitialDeposit) : IMessage;
    public record AccountCredited(Guid AccountId, decimal Amount) : IMessage;
    public record AccountDebited(Guid AccountId, decimal Amount) : IMessage;
}
