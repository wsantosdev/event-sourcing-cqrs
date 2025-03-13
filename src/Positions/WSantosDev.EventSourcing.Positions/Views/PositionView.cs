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

        public static PositionView CreateFrom(Position position) =>
            new (position.AccountId, position.Symbol, position.Available);

        public void UpdateFrom(Position position) =>
            Available = position.Available;
    }
}
