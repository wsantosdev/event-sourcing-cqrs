namespace WSantosDev.EventSourcing.Commons
{
    public sealed class AccountId
    {
        public static readonly AccountId Empty = new(Guid.Empty);

        public Guid Value { get; init; }

        private AccountId(Guid value) =>
            Value = value;

        public AccountId() { }

        public static AccountId New() =>
            new(Guid.NewGuid());

        public static implicit operator AccountId(Guid guid) =>
            new(guid);

        public static implicit operator Guid(AccountId accountId) =>
            accountId.Value;

        public override string ToString() =>
            Value.ToString();
    }
}