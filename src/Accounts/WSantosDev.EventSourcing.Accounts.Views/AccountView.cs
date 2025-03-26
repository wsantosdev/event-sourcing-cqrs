namespace WSantosDev.EventSourcing.Accounts
{
    public class AccountView(Guid accountId, decimal balance)
    {
        public Guid AccountId { get; init; } = accountId;
        public decimal Balance { get; set; } = balance;
    }
}
