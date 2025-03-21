using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Accounts.Entities
{
    public record AccountSnapshot(AccountId AccountId, IEnumerable<AccountEntry> Entries, long Version) : ISnapshot;
}
