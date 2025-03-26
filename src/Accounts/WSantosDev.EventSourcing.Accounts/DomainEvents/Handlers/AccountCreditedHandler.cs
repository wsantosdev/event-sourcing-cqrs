using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Accounts
{
    public class AccountCreditedHandler(AccountViewDbContext viewDbContext) : IMessageHandler<AccountCredited>
    {
        public async Task HandleAsync(AccountCredited message)
        {
            var stored = await viewDbContext.ByAccountIdAsync(message.AccountId);
            if (stored)
            {
                var view = stored.Get();
                view.Balance += message.Amount;
                viewDbContext.Update(view);
            }
            else
            {
                var view = AccountView.Create(message.AccountId, message.Amount);
                viewDbContext.Add(view);
            }

            await viewDbContext.SaveChangesAsync();
        }
    }
}
