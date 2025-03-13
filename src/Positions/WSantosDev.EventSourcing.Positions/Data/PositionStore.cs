using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Positions
{
    public class PositionStore(SqliteConfig config)
    {
        public async Task<Option<Position>> BySymbolAsync(AccountId accountId, Symbol symbol, CancellationToken cancellationToken = default)
        {
            var eventDbContext = new EventDbContext(new DbContextOptionsBuilder<EventDbContext>().UseSqlite(config.ConnectionString).Options);
            var stream = await eventDbContext.ReadStreamAsync(StreamId(accountId, symbol), cancellationToken);

            return stream.Any()
                ? Position.Restore(stream)
                : Option.None<Position>();
        }

        public async Task<Result<IError>> StoreAsync(Position position, CancellationToken cancellationToken = default)
        {
            using var sqliteConnection = new SqliteConnection(config.ConnectionString);

            EventDbContext eventDbContext = new(new DbContextOptionsBuilder<EventDbContext>().UseSqlite(sqliteConnection).Options);
            using var transaction = await eventDbContext.Database.BeginTransactionAsync(cancellationToken);
            
            try
            {
                eventDbContext.AppendToStream(StreamId(position.AccountId, position.Symbol), position.UncommittedEvents);
                await eventDbContext.SaveChangesAsync(cancellationToken);
                
                PositionViewDbContext viewDbContext = new (new DbContextOptionsBuilder<PositionViewDbContext>().UseSqlite(sqliteConnection).Options);
                await viewDbContext.Database.UseTransactionAsync(transaction.GetDbTransaction(), cancellationToken);
                var stored = await viewDbContext.BySymbolAsync(position.AccountId, position.Symbol, cancellationToken);
                if (stored)
                {
                    var view = stored.Get();
                    view.UpdateFrom(position);
                    viewDbContext.Update(view);
                }
                else
                { 
                    var view = PositionView.CreateFrom(position);
                    await viewDbContext.AddAsync(view, cancellationToken);
                }

                await viewDbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return true;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                return PositionStoreErrors.StorageUnavailable;
            }
        }
    
        private static string StreamId(AccountId accountId, Symbol symbol) =>
            $"Position_{accountId}_{symbol}";
    }
}
