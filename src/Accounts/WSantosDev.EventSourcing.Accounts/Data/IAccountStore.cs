using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Accounts
{
    public interface IAccountStore
    {
        Option<Account> GetById(AccountId accountId);
        
        Task<Result<IError>> StoreAsync(Account account);
    }
}
