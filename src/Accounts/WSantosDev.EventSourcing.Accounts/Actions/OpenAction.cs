using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Accounts.Actions
{
    public class OpenAction(IAccountReadModelStore readModelStore, IAccountStore store, IMessageBus messageBus)
    {
        public Result<IError> Execute(OpenActionParams command)
        {
            var stored = readModelStore.GetById(command.AccountId);
            if (stored)
                return ActionErrors.AccountAlreadyExists;
            
            var opened = Account.Open(command.AccountId, command.InitialDeposit);
            if (opened)
            {
                store.Store(opened);
                messageBus.Publish(new DomainEvents.AccountOpened(command.AccountId, command.InitialDeposit));
                return Result<IError>.Ok();
            }

            return Result<IError>.Error(opened.ErrorValue);
        }
    }

    public record OpenActionParams(AccountId AccountId, Money InitialDeposit);
}
