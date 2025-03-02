using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Accounts.DomainEvents
{
    public class AccountOpenedHandler(IAccountReadModelStore store) : IMessageHandler<AccountOpened>
    {
        public void Handle(AccountOpened @event) =>
            store.StoreAsync(new AccountReadModel(@event.AccountId, @event.InitialDeposit));
    }
}
