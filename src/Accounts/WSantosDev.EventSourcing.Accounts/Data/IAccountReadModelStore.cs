using Moonad;
using WSantosDev.EventSourcing.Commons;

namespace WSantosDev.EventSourcing.Accounts
{
    public interface IAccountReadModelStore
    {
        Task<Option<AccountReadModel>> ByIdAsync(AccountId accountId);
        
        Task StoreAsync(AccountReadModel account);
    }
}
