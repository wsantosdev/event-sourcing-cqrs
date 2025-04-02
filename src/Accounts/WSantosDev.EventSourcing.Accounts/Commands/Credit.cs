using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Accounts.Commands
{
    public class Credit(AccountStore store, IMessageBus messageBus)
    {
        public async Task<Result<IError>> ExecuteAsync(CreditParams @params)
        {
            var stored = await store.ByIdAsync(@params.AccountId);
            if (!stored)
                return CommandErrors.AccountNotFound;

            var account = stored.Get();
            var credited = account.Credit(@params.Amount);
            if (!credited)
                return credited;

            var persisted = await store.StoreAsync(account);
            if (!persisted)
                return persisted;

            if (account.ShouldTakeSnapshot())
                await store.StoreSnapshotAsync(account.TakeSnapshot());

            messageBus.Publish(new AccountCredited(@params.AccountId, @params.Amount));
            return true;
        }
    }

    public record CreditParams(AccountId AccountId, Money Amount);
}
