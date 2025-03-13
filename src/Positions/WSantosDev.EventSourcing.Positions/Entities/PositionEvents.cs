using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Positions
{
    public sealed partial class Position
    {
        private record struct PositionOpened(AccountId AccountId, string Symbol) : IEvent;
        private record struct SharesDeposited(decimal Quantity) : IEvent;
        private record struct SharesWithdrawn(decimal Quantity) : IEvent;
    }
}
