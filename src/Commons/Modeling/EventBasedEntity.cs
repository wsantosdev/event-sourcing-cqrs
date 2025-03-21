namespace WSantosDev.EventSourcing.Commons.Modeling
{
    public abstract class EventBasedEntity
    {
        protected long Version { get; set; }
        public Dictionary<long, IEvent> UncommittedEvents { get; } = new ();
        
        protected void RaiseEvent<TEvent>(TEvent @event) where TEvent : IEvent
        {
            UncommittedEvents.Add(++Version, @event);
            ProcessEvent(@event);
        }

        protected void FeedEvents(IEnumerable<IEvent> stream)
        {
            foreach (var @event in stream)
            {
                Version++;
                ProcessEvent(@event);
            }
        }

        protected abstract void ProcessEvent(IEvent @event);
    }
}
