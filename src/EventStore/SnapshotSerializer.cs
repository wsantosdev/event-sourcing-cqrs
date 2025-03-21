using System.Text.Json;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.EventStore
{
    internal sealed class SnapshotSerializer
    {
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
            return AppDomain.CurrentDomain
                            .GetAssemblies()
                            .SelectMany(a => a.GetTypes().Where(x => x.FullName == typeName))
                            .First();
        }
    }
}
