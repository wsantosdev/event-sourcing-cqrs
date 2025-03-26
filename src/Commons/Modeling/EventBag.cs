namespace WSantosDev.EventSourcing.Commons.Modeling
{
    public record EventBag(long EntityVersion, IEvent Event);
}
