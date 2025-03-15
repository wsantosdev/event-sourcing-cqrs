namespace WSantosDev.EventSourcing.Positions
{
    public class PositionView
    { 
        public Guid AccountId { get; private set; }
        public string Symbol { get; private set; }
        public int Available { get; private set; }

        private PositionView(Guid accountId, string symbol, int available)
        { 
            AccountId = accountId;
            Symbol = symbol;
            Available = available;
        }

        public static PositionView CreateFrom(Position source) =>
            new (source.AccountId, source.Symbol, source.Available);

        public void UpdateFrom(Position source) =>
            Available = source.Available;
    }
}
