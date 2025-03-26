using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Modeling;
using WSantosDev.EventSourcing.SharedStorage;

namespace WSantosDev.EventSourcing.Exchange
{
    public sealed class ExchangeOrderStore(EventDbContext eventDbContext, ExchangeOrderViewDbContext viewDbContext)
    {
        public async Task<Option<ExchangeOrder>> ByIdAsync(OrderId orderId, CancellationToken cancellationToken = default)
        {
            var stream = await eventDbContext.ReadStreamAsync(StreamId(orderId), cancellationToken);
                
            return stream.Any()
                ? new ExchangeOrder(stream)
                : Option.None<ExchangeOrder>();
        }

        public async Task<Result<IError>> StoreAsync(ExchangeOrder exchangeOrder, CancellationToken cancellationToken = default)
        {
            
            using var transaction = await eventDbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                eventDbContext.AppendToStream(StreamId(exchangeOrder.OrderId), exchangeOrder.UncommittedEvents);
                await eventDbContext.SaveChangesAsync(cancellationToken);

                viewDbContext.Database.SetDbConnection(eventDbContext.Database.GetDbConnection());
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

