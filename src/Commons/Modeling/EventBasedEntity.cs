namespace WSantosDev.EventSourcing.Commons.Modeling
{
    public abstract class EventBasedEntity
    {
        private long Index { get; set; }
        public Dictionary<long, IEvent> UncommittedEvents { get; } = new ();
        
        protected void RaiseEvent<TEvent>(TEvent @event) where TEvent : IEvent
        {
            UncommittedEvents.Add(++Index, @event);
            ProcessEvent(@event);
        }

        protected void Restore(IEnumerable<IEvent> stream)
        {
            Index = stream.Count();

            foreach (var @event in stream)
                ProcessEvent(@event);
        }

        protected abstract void ProcessEvent(IEvent @event);
    }
}
