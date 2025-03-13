using WSantosDev.EventSourcing.Commons;

namespace WSantosDev.EventSourcing.Accounts
{
    public class AccountView(Guid accountId, decimal balance)
    {
        public Guid AccountId { get; init; } = accountId;
        public decimal Balance { get; private set; } = balance;

        public void UpdateFrom(Account source) =>
            Balance = source.Balance;

        public static AccountView CreateFrom(Account source) =>
            new(source.AccountId, source.Balance);
    }
}
