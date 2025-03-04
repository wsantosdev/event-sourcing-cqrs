using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Accounts.Commands
{
    public class Open(IAccountStore store, IMessageBus messageBus)
    {
        public async Task<Result<IError>> ExecuteAsync(OpenActionParams @params)
        {
            var stored = await store.GetByIdAsync(@params.AccountId);
            if (stored)
                return CommandErrors.AccountAlreadyExists;
            
            var opened = Account.Open(@params.AccountId, @params.InitialDeposit);
            if (opened)
            {
                await store.StoreAsync(opened);
                messageBus.Publish(new DomainEvents.AccountOpened(@params.AccountId, @params.InitialDeposit));
                return true;
            }

            return Result<IError>.Error(opened.ErrorValue);
        }
    }

    public record OpenActionParams(AccountId AccountId, Money InitialDeposit);
}
