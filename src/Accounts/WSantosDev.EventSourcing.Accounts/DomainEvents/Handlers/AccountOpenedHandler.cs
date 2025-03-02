using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Accounts.DomainEvents
{
    public class AccountOpenedHandler(IAccountReadModelStore store) : IMessageHandler<AccountOpened>
    {
        public async Task HandleAsync(AccountOpened @event) =>
            await store.StoreAsync(new AccountReadModel(@event.AccountId, @event.InitialDeposit));
    }
}
