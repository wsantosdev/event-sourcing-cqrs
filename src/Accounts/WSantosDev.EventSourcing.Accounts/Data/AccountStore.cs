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
            var stored = await snapshotDbContext.ByIdAsync<AccountSnapshot>(StreamId(accountId), cancellationToken);
            if (stored)
            {
                var snapshot = stored.Get();
                var streamFromId = await eventDbContext.ReadStreamFromEventIdAsync(StreamId(accountId), snapshot.EntityVersion, cancellationToken);
                var account = Account.Restore(snapshot, streamFromId);

                return account;
            }
            
            var stream = await eventDbContext.ReadStreamAsync(StreamId(accountId), cancellationToken);
                return stream.Any()
                    ? Account.Restore(stream)
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

        public async Task<Result<IError>> StoreSnapshotAsync(AccountSnapshot snapshot, CancellationToken cancellationToken = default)
        {
            try
            {
                await snapshotDbContext.UpsertAsync(StreamId(snapshot.AccountId), snapshot, cancellationToken);
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
