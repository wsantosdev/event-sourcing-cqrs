using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Accounts.DomainEvents
{
    public class AccountUpdatedHandler(IAccountReadModelStore store) : IMessageHandler<AccountUpdated>
    {
        public async Task HandleAsync(AccountUpdated @event) =>
            await store.StoreAsync(new AccountReadModel(@event.AccountId, @event.Balance));
    }
}
