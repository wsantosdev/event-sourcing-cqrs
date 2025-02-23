using Moonad;
using WSantosDev.EventSourcing.Commons;

namespace WSantosDev.EventSourcing.Accounts
{
    public interface IAccountStore
    {
        Option<Account> GetById(AccountId accountId);
        
        void Store(Account account);
    }
}
