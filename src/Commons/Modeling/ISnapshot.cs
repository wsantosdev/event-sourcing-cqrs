namespace WSantosDev.EventSourcing.Commons.Modeling
{
    public interface ISnapshot
    {
        int EntityVersion { get; }
    }
}
