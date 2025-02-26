using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Accounts.Actions
{
    public class DebitAction(IAccountStore store, IMessageBus messageBus)
    {
        public Result<IError> Execute(DebitActionParams @params)
        {
            var stored = store.GetById(@params.AccountId);
            if (stored)
            {
                var account = stored.Get();
                var debited = account.Debit(@params.Amount);
                if (debited)
                {
                    store.Store(account);
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
