using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Accounts
{
    public sealed class AccountStore(SqliteConfig config)
    {
        public async Task<Option<Account>> ByIdAsync(AccountId accountId, CancellationToken cancellationToken = default)
        {
            var eventDbContext = new EventDbContext(new DbContextOptionsBuilder<EventDbContext>().UseSqlite(config.ConnectionString).Options);
            var stream = await eventDbContext.ReadStreamAsync(StreamId(accountId), cancellationToken);
            
            return stream.Any()
                ? Account.Restore(stream)
                : Option.None<Account>();
        }

        public async Task<Result<IError>> StoreAsync(Account account, CancellationToken cancellationToken = default)
        {
            using var sqliteConnection = new SqliteConnection(config.ConnectionString);

            EventDbContext eventDbContext = new (new DbContextOptionsBuilder<EventDbContext>().UseSqlite(sqliteConnection).Options);
            using var transaction = await eventDbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                eventDbContext.AppendToStream(StreamId(account.AccountId), account.UncommittedEvents);
                await eventDbContext.SaveChangesAsync(cancellationToken);

                AccountViewDbContext viewDbContext = new (new DbContextOptionsBuilder<AccountViewDbContext>().UseSqlite(sqliteConnection).Options);
                await viewDbContext.Database.UseTransactionAsync(transaction.GetDbTransaction(), cancellationToken);
                
                var stored = await viewDbContext.ByIdAsync(account.AccountId, cancellationToken);
                if (stored)
                {
                    var readModel = stored.Get();
                    readModel.UpdateFrom(account);
                    viewDbContext.Update(readModel);
                }
                else
                {
                    var readModel = AccountView.CreateFrom(account);
                    await viewDbContext.AddAsync(readModel, cancellationToken);
                }
                
                await viewDbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return true;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                return AccountStoreErrors.StorageUnavailable;
            }
        }

        private static string StreamId(AccountId accountId) =>
            $"Account_{accountId.Value}";
    }
}
