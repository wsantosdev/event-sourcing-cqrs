﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Orders
{
    public sealed class OrderStore(SqliteConfig config)
    {
        public async Task<Option<Order>> GetByIdAsync(OrderId orderId, CancellationToken cancellationToken = default)
        {
            var eventDbContext = new EventDbContext(new DbContextOptionsBuilder<EventDbContext>().UseSqlite(config.ConnectionString).Options);
            var stream = await eventDbContext.ReadStreamAsync(StreamId(orderId), cancellationToken);

            return stream.Any()
                ? Order.Restore(stream)
                : Option.None<Order>();
        }

        public async Task<Result<IError>> StoreAsync(Order order, CancellationToken cancellationToken = default)
        {
            using var sqliteConnection = new SqliteConnection(config.ConnectionString);

            EventDbContext eventDbContext = new(new DbContextOptionsBuilder<EventDbContext>().UseSqlite(sqliteConnection).Options);
            using var transaction = await eventDbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                eventDbContext.AppendToStream(StreamId(order.OrderId), order.UncommittedEvents);
                await eventDbContext.SaveChangesAsync(cancellationToken);

                OrderViewDbContext viewDbContext = new(new DbContextOptionsBuilder<OrderViewDbContext>().UseSqlite(sqliteConnection).Options);
                await viewDbContext.Database.UseTransactionAsync(transaction.GetDbTransaction(), cancellationToken);

                var stored = await viewDbContext.ByOrderIdAsync(order.OrderId, cancellationToken);
                if (stored)
                {
                    var view = stored.Get();
                    view.UpdateFrom(order);
                    viewDbContext.Update(view);
                }
                else
                { 
                    await viewDbContext.AddAsync(OrderView.CreateFrom(order), cancellationToken);
                }

                await viewDbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return true;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                return OrderStoreErrors.StorageUnavailable;
            }
        }
        
        private static string StreamId(OrderId orderId) =>
            $"Order_{orderId}";
    }
}
