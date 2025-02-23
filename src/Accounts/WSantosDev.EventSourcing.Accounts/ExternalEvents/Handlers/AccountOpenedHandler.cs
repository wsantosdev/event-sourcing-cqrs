using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Accounts.ExternalEvents
{
    public class AccountOpenedHandler(IAccountReadModelStore store) : IMessageHandler<AccountOpened>
    {
        public void Handle(AccountOpened @event) =>
            store.Store(new AccountReadModel(@event.AccountId, @event.InitialDeposit));
    }
}
