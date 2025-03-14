namespace WSantosDev.EventSourcing.EventStore
{
    public static class EventIndex
    {
        private static long _index = 0;

        public static long Next() =>
            Interlocked.Increment(ref _index);

        public static void Seed(long value) =>
            Interlocked.Exchange(ref _index, value);
    }
}
