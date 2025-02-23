using Moonad;
using WSantosDev.EventSourcing.Commons;

namespace WSantosDev.EventSourcing.Accounts.Queries
{
    public class AccountQuery(IAccountReadModelStore readModelStore)
    {
        public Option<AccountReadModel> Execute(AccountQueryParams queryParams) =>
            readModelStore.GetById(queryParams.AccountId);
    }

    public record AccountQueryParams(AccountId AccountId);
}
