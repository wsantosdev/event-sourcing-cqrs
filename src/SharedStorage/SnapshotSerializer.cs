using System.Text.Json;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.SharedStorage
{
    internal sealed class SnapshotSerializer
    {
        private static readonly Dictionary<string, Type> _types = [];

        public static Snapshot Serialize(string snapshotId, ISnapshot snapshot)
        {
            var type = snapshot.GetType();
            
            return new(snapshotId,
                       type.FullName!,
                       snapshot.Version,
                       DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                       string.Empty,
                       JsonSerializer.Serialize(snapshot, type));
        }

        public static ISnapshot Deserialize(Snapshot snapshot) =>
            (ISnapshot)JsonSerializer.Deserialize(snapshot.Data, GetType(snapshot.Type))!;

        private static Type GetType(string typeName)
        {
            if (_types.TryGetValue(typeName, out var cached))
                return cached;

            var type = AppDomain.CurrentDomain
                                .GetAssemblies()
                                .SelectMany(a => a.GetTypes().Where(x => x.FullName == typeName))
                                .First();

            _types.Add(typeName, type);
            return type;
        }
    }
}
