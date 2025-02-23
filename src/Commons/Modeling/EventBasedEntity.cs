namespace WSantosDev.EventSourcing.Commons.Modeling
{
    public abstract class EventBasedEntity
    {
        private readonly Queue<IEvent> _uncommittedEvents = new();
        
        public IReadOnlyCollection<IEvent> UncommittedEvents =>
            _uncommittedEvents.ToList().AsReadOnly();
        
        protected void RaiseEvent<TEvent>(TEvent @event) where TEvent : IEvent
        {
            _uncommittedEvents.Enqueue(@event);
            ProcessEvent(@event);
        }

        protected void Hydrate(IEnumerable<IEvent> stream)
        {
            foreach (var @event in stream)
                ProcessEvent(@event);
        }

        protected abstract void ProcessEvent(IEvent @event);
    }
}
