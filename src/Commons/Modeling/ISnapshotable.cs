namespace WSantosDev.EventSourcing.Commons.Modeling
{
    public interface ISnapshotable<T>
    {
        bool ShouldTakeSnapshot();

        T TakeSnapshot();
    }
}
