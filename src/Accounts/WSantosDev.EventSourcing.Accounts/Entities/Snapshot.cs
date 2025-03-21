using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Accounts
{
    public record Snapshot(AccountId AccountId, IEnumerable<Entry> Entries, long Version) : ISnapshot;
}
