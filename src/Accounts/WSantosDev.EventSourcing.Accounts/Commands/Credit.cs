using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Accounts.Commands
{
    public class Credit(IAccountStore store, IMessageBus messageBus)
    {
        public async Task<Result<IError>> ExecuteAsync(CreditActionParams @params)
        {
            var stored = await store.GetByIdAsync(@params.AccountId);
            if (stored)
            {
                var account = stored.Get();
                var credited = account.Credit(@params.Amount);
                if (credited)
                {
                    await store.StoreAsync(account);
                    messageBus.Publish(new DomainEvents.AccountUpdated(account.AccountId, account.Balance));
                    return true;
                }

                return Result<IError>.Error(credited.ErrorValue);
            }

            return CommandErrors.AccountNotFound;
        }
    }

    public record CreditActionParams(AccountId AccountId, Money Amount);
}
