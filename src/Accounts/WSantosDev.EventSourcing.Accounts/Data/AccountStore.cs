﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Modeling;
using WSantosDev.EventSourcing.EventStore;

namespace WSantosDev.EventSourcing.Accounts
{
    public sealed class AccountStore(EventDbContext eventDbContext, AccountViewDbContext viewDbContext)
    {
        public async Task<Option<Account>> ByIdAsync(AccountId accountId, CancellationToken cancellationToken = default)
        {
            var stream = await eventDbContext.ReadStreamAsync(StreamId(accountId), cancellationToken);
            
            return stream.Any()
                ? new Account(stream)
                : Option.None<Account>();
        }

        public async Task<Result<IError>> StoreAsync(Account account, CancellationToken cancellationToken = default)
        {
            using var transaction = await eventDbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                eventDbContext.AppendToStream(StreamId(account.AccountId), account.UncommittedEvents);
                await eventDbContext.SaveChangesAsync(cancellationToken);

                viewDbContext.Database.SetDbConnection(eventDbContext.Database.GetDbConnection());
                await viewDbContext.Database.UseTransactionAsync(transaction.GetDbTransaction(), cancellationToken);
                
                var stored = await viewDbContext.ByAccountIdAsync(account.AccountId, cancellationToken);
                if (stored)
                {
                    var view = stored.Get();
                    view.UpdateFrom(account);
                    viewDbContext.Update(view);
                }
                else
                {
                    var view = AccountView.CreateFrom(account);
                    await viewDbContext.AddAsync(view, cancellationToken);
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
