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
            if (stored)
            {
                var account = stored.Get();
                var credited = account.Credit(@params.Amount);
                if (credited)
                {
                    var persisted = await store.StoreAsync(account);
                    if (persisted) 
                    {
                        if (account.ShouldTakeSnapshot())
                            await store.StoreSnapshotAsync(account.TakeSnapshot());

                        messageBus.Publish(new AccountCredited(@params.AccountId, @params.Amount));
                    }

                    return persisted;
                }
                
                return Result<IError>.Error(credited.ErrorValue);
            }

            return CommandErrors.AccountNotFound;
        }
    }

    public record CreditParams(AccountId AccountId, Money Amount);
}
