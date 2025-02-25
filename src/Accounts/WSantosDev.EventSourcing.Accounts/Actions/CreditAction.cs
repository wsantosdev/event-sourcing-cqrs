using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Accounts.Actions
{
    public class CreditAction(IAccountStore store, IMessageBus messageBus)
    {
        public Result<IError> Execute(CreditActionParams command)
        {
            var stored = store.GetById(command.AccountId);
            if (stored)
            {
                var account = stored.Get();
                var credited = account.Credit(command.Amount);
                if (credited)
                {
                    store.Store(account);
                    messageBus.Publish(new DomainEvents.AccountUpdated(account.AccountId, account.Balance));
                    return Result<IError>.Ok();
                }

                return Result<IError>.Error(credited.ErrorValue);
            }

            return ActionErrors.AccountNotFound;
        }
    }

    public record CreditActionParams(AccountId AccountId, Money Amount);
}
