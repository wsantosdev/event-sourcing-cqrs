namespace WSantosDev.EventSourcing.Commons.Modeling
{
    public abstract class EventBasedEntity
    {
        protected long Version { get; set; }
        public List<EventBag> UncommittedEvents { get; } = new ();
        
        protected void RaiseEvent<TEvent>(TEvent @event) where TEvent : IEvent
        {
            UncommittedEvents.Add(new EventBag(++Version, @event));
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
