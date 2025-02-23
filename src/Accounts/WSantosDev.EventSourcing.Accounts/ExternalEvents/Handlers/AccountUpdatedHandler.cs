using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Accounts.ExternalEvents
{
    public class AccountUpdatedHandler(IAccountReadModelStore store) : IMessageHandler<AccountUpdated>
    {
        public void Handle(AccountUpdated @event) =>
            store.Update(new AccountReadModel(@event.AccountId, @event.Balance));
    }
}
