using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Modeling;

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

                ExchangeOrderViewDbContext readModelDbContext = new (new DbContextOptionsBuilder<ExchangeOrderViewDbContext>().UseSqlite(sqliteConnection).Options);
                await readModelDbContext.Database.UseTransactionAsync(transaction.GetDbTransaction(), cancellationToken);

                var view = new ExchangeOrderView(exchangeOrder.AccountId, exchangeOrder.OrderId, exchangeOrder.Side,
                                                 exchangeOrder.Quantity, exchangeOrder.Symbol, exchangeOrder.Price,
                                                 exchangeOrder.Status);

                var stored = (await readModelDbContext.ExchangeOrders.FirstOrDefaultAsync(o => o.OrderId == exchangeOrder.OrderId, cancellationToken)).ToOption();
                if (stored)
                {
                    readModelDbContext.Entry(stored.Get()).State = EntityState.Detached;
                    readModelDbContext.Entry(view).State = EntityState.Modified;
                }
                else
                {
                    await readModelDbContext.AddAsync(view, cancellationToken);
                }
                
                await readModelDbContext.SaveChangesAsync(cancellationToken);
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

