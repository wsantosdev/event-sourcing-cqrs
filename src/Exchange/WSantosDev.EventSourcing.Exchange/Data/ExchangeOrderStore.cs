using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Modeling;
using WSantosDev.EventSourcing.EventStore;

namespace WSantosDev.EventSourcing.Exchange
{
    public sealed class ExchangeOrderStore(SqliteConfig config)
    {
        public async Task<Option<ExchangeOrder>> ByIdAsync(OrderId orderId, CancellationToken cancellationToken = default)
        {
            var dbContext = new EventDbContext(new DbContextOptionsBuilder<EventDbContext>().UseSqlite(config.ConnectionString).Options);
            var stream = await dbContext.ReadStreamAsync(StreamId(orderId), cancellationToken);
                
            return stream.Any()
                ? ExchangeOrder.Restore(stream)
                : Option.None<ExchangeOrder>();
        }

        public async Task<Result<IError>> StoreAsync(ExchangeOrder exchangeOrder, CancellationToken cancellationToken = default)
        {
            using var sqliteConnection = new SqliteConnection(config.ConnectionString);

            EventDbContext eventDbContext = new (new DbContextOptionsBuilder<EventDbContext>().UseSqlite(sqliteConnection).Options);
            using var transaction = await eventDbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                eventDbContext.AppendToStream(StreamId(exchangeOrder.OrderId), exchangeOrder.UncommittedEvents);
                await eventDbContext.SaveChangesAsync(cancellationToken);

                ExchangeOrderViewDbContext viewDbContext = new (new DbContextOptionsBuilder<ExchangeOrderViewDbContext>().UseSqlite(sqliteConnection).Options);
                await viewDbContext.Database.UseTransactionAsync(transaction.GetDbTransaction(), cancellationToken);
                                
                var stored = await viewDbContext.ByOrderIdAsync(exchangeOrder.OrderId, cancellationToken);
                if (stored)
                {
                    var view = stored.Get();
                    view.UpdateFrom(exchangeOrder);
                    viewDbContext.Update(view);
                }
                else
                {
                    var view = ExchangeOrderView.CreateFrom(exchangeOrder);
                    await viewDbContext.AddAsync(view, cancellationToken);
                }
                
                await viewDbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return true;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                return ExchangeOrderStoreErrors.StorageUnavailable;
            }
        }

        private static string StreamId(OrderId orderId) =>
            $"ExchangeOrder_{orderId}";
    }
}

