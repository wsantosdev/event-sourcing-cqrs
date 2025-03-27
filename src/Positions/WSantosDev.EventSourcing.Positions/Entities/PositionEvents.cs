using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Positions
{
    public sealed partial class Position
    {
        private record struct PositionOpened(int Id, AccountId AccountId, string Symbol) : IEvent;
        private record struct SharesDeposited(int Id, decimal Quantity) : IEvent;
        private record struct SharesWithdrawn(int Id, decimal Quantity) : IEvent;
    }
}
