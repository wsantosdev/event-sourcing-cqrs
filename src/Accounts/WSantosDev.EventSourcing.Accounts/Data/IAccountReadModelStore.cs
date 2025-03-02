using Moonad;
using WSantosDev.EventSourcing.Commons;

namespace WSantosDev.EventSourcing.Accounts
{
    public interface IAccountReadModelStore
    {
        Option<AccountReadModel> GetById(AccountId accountId);
        
        void Store(AccountReadModel account);
    }
}
