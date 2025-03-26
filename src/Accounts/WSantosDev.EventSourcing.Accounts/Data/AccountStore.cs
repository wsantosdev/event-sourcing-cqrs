using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Modeling;
using WSantosDev.EventSourcing.SharedStorage;

namespace WSantosDev.EventSourcing.Accounts
{
    public sealed class AccountStore(EventDbContext eventDbContext, SnapshotDbContext snapshotDbContext)
    {
        public async Task<Option<Account>> ByIdAsync(AccountId accountId, CancellationToken cancellationToken = default)
        {
            var snapshot = await snapshotDbContext.ByIdAsync(StreamId(accountId), cancellationToken);
            if (snapshot)
            {
                var streamFromId = await eventDbContext.ReadStreamFromEventIdAsync(StreamId(accountId), snapshot.Get().Version, cancellationToken);
                var account = Account.Restore(snapshot.Get(), streamFromId);

                return account;
            }
            
            var stream = await eventDbContext.ReadStreamAsync(StreamId(accountId), cancellationToken);
                return stream.Any()
                    ? new Account(stream)
                    : Option.None<Account>();
        }

        public async Task<Result<IError>> StoreAsync(Account account, CancellationToken cancellationToken = default)
        {
            try
            {
                eventDbContext.AppendToStream(StreamId(account.AccountId), account.UncommittedEvents);
                await eventDbContext.SaveChangesAsync(cancellationToken);

                return true;
            }
            catch
            {
                return AccountStoreErrors.StorageUnavailable;
            }
        }

        public async Task<Result<IError>> StoreSnapshotAsync(ISnapshot snapshot, CancellationToken cancellationToken = default)
        {
            try
            {
                var accountId = ((AccountSnapshot)snapshot).AccountId;

                await snapshotDbContext.UpsertAsync(StreamId(accountId), snapshot, cancellationToken);
                await snapshotDbContext.SaveChangesAsync(cancellationToken);

                return true;
            }
            catch
            {
                return AccountStoreErrors.StorageUnavailable;
            }
        }

        private static string StreamId(AccountId accountId) =>
            $"Account_{accountId.Value}";
    }
}
