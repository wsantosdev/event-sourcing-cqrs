using WSantosDev.EventSourcing.Commons.Messaging;

namespace WSantosDev.EventSourcing.Accounts
{
    public class AccountDebitedHandler(AccountViewDbContext viewDbContext) : IMessageHandler<AccountDebited>
    {
        public async Task HandleAsync(AccountDebited message)
        {
            var stored = await viewDbContext.ByAccountIdAsync(message.AccountId);
            if (stored)
            {
                var view = stored.Get();
                view.Balance -= message.Amount;
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
