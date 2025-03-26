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
                return CommandErrors.AccountAlreadyOpened;
            
            var opened = Account.Open(@params.AccountId, @params.InitialDeposit);
            if (opened)
            {
                var persisted = await store.StoreAsync(opened);
                if (persisted)
                    messageBus.Publish(new AccountOpened(@params.AccountId, @params.InitialDeposit));

                return persisted;
            }

            return Result<IError>.Error(opened.ErrorValue);
        }
    }

    public record OpenParams(AccountId AccountId, Money InitialDeposit);
}
