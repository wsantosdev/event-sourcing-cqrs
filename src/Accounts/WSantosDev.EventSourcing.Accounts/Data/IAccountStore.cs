using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Accounts
{
    public interface IAccountStore
    {
        Task<Option<Account>> GetByIdAsync(AccountId accountId);
        
        Task<Result<IError>> StoreAsync(Account account);
    }
}
