using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Accounts.Commands
{
    public class Debit(AccountStore store, IMessageBus messageBus)
    {
        public async Task<Result<IError>> ExecuteAsync(DebitParams @params)
        {
            var stored = await store.ByIdAsync(@params.AccountId);
            if (!stored)
                return CommandErrors.AccountNotFound;
                
            var account = stored.Get();
            var debited = account.Debit(@params.Amount);
            if (!debited)
                return debited;

            var persisted = await store.StoreAsync(account);
            if (!persisted)
                return persisted;
            
            if (account.ShouldTakeSnapshot())
                await store.StoreSnapshotAsync(account.TakeSnapshot());
            
            messageBus.Publish(new AccountDebited(@params.AccountId, @params.Amount));
            return true;
        }
    }

    public record DebitParams(AccountId AccountId, Money Amount);
}
