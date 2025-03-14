using System.Text.Json;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.EventStore
{
    internal sealed class EventSerializer
    {
        public static Event Serialize(long eventId, string streamId, IEvent @event)
        {
            return new(eventId,
                       streamId,
                       DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                       string.Empty,
                       @event.GetType().FullName!,
                       JsonSerializer.Serialize(@event, @event.GetType()));
        }

        public static IEvent Desserialize(Event @event) =>
            (IEvent)JsonSerializer.Deserialize(@event.Data, GetType(@event.EventType))!;

        private static Type GetType(string typeName)
        {
            return AppDomain.CurrentDomain
                            .GetAssemblies()
                            .SelectMany(a => a.GetTypes().Where(x => x.FullName == typeName))
                            .First();
        }
    }
}
