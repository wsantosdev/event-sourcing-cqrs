using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Accounts
{
    public interface IAccountStore
    {
        Option<Account> GetById(AccountId accountId);
        
        Result<IError> Store(Account account);
    }
}
