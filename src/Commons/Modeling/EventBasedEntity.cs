namespace WSantosDev.EventSourcing.Commons.Modeling
{
    public abstract class EventBasedEntity
    {
        public int Version { get; protected set; } = 1;
        public List<IEvent> UncommittedEvents { get; } = new ();
        
        protected void RaiseEvent<TEvent>(TEvent @event) where TEvent : IEvent
        {
            Version++;
            UncommittedEvents.Add(@event);
            ProcessEvent(@event);
        }

        protected void FeedEvents(IEnumerable<IEvent> stream)
        {
            foreach (var @event in stream)
            {
                Version = @event.Id;
                ProcessEvent(@event);
            }

            Version++;
        }

        protected abstract void ProcessEvent(IEvent @event);
    }
}
