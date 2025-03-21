namespace WSantosDev.EventSourcing.Commons.Modeling
{
    public interface ISnapshotable
    {
        bool ShouldTakeSnapshot();

        ISnapshot TakeSnapshot();
    }
}
