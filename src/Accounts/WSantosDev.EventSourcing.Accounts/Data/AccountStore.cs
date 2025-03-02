using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Accounts
{
    public sealed class AccountStore(EventStore eventStore) : IAccountStore
    {
        public async Task<Option<Account>> GetByIdAsync(AccountId accountId)
        {
            var stream = await eventStore.ReadAsync(StreamId(accountId));
            
            return stream.Any()
                ? Account.Restore(stream)
                : Option.None<Account>();
        }

        public async Task<Result<IError>> StoreAsync(Account account) =>
            await eventStore.AppendAsync(StreamId(account.AccountId), account.UncommittedEvents);
            

        private static string StreamId(AccountId accountId) =>
            $"Account_{accountId.Value}";
    }
}
