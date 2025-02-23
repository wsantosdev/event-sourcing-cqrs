namespace WSantosDev.EventSourcing.Positions
{
    public record PositionReadModel(Guid AccountId, string Symbol, int Available);
}
