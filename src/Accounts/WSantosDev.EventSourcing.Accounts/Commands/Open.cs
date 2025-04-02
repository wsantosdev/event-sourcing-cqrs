using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Messaging;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Accounts.Commands
{
    public class Open(AccountStore store, IMessageBus messageBus)
    {
        public async Task<Result<IError>> ExecuteAsync(OpenParams @params)
        {
            var stored = await store.ByIdAsync(@params.AccountId);
            if (stored)
                return CommandErrors.AccountAlreadyOpen;
            
            var opened = Account.Open(@params.AccountId, @params.InitialDeposit);
            if (!opened)
                return Result<IError>.Error(opened.ErrorValue);

            var persisted = await store.StoreAsync(opened);
            if (!persisted)
                return persisted;

            messageBus.Publish(new AccountOpened(@params.AccountId, @params.InitialDeposit));
            return true;
        }
    }

    public record OpenParams(AccountId AccountId, Money InitialDeposit);
}
