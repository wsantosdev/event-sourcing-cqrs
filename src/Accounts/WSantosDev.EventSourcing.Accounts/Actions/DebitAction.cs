using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Accounts.Actions
{
    public class DebitAction(IAccountStore store, IMessageBus messageBus)
    {
        public async Task<Result<IError>> ExecuteAsync(DebitActionParams @params)
        {
            var stored = await store.GetByIdAsync(@params.AccountId);
            if (stored)
            {
                var account = stored.Get();
                var debited = account.Debit(@params.Amount);
                if (debited)
                {
                    await store.StoreAsync(account);
                    messageBus.Publish(new DomainEvents.AccountUpdated(account.AccountId, account.Balance));
                    return true;
                }

                return Result<IError>.Error(debited.ErrorValue);
            }

            return ActionErrors.AccountNotFound;
        }
    }

    public record DebitActionParams(AccountId AccountId, Money Amount);
}
