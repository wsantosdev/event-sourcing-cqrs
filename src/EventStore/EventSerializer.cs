﻿using System.Text.Json;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.EventStore
{
    internal sealed class EventSerializer
    {
        public static Event Serialize(long eventId, string streamId, IEvent @event)
        {
            var type = @event.GetType();

            return new(eventId,
                       streamId,
                       DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                       string.Empty,
                       type.FullName!,
                       JsonSerializer.Serialize(@event, type));
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
