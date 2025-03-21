namespace WSantosDev.EventSourcing.Commons.Modeling
{
    public interface ISnapshot
    {
        long Version { get; }
    }
}
