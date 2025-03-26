using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Accounts
{
    public class AccountOpenedHandler(AccountViewDbContext viewDbContext) : IMessageHandler<AccountOpened>
    {
        public async Task HandleAsync(AccountOpened message)
        {
            var view = AccountView.Create(message.AccountId, message.InitialDeposit);
            viewDbContext.Add(view);

            await viewDbContext.SaveChangesAsync();
        }
    }
}
