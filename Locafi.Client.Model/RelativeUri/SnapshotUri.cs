using System;

namespace Locafi.Client.Model.RelativeUri
{
    public static class SnapshotUri
    {
        public static string ServiceName => "Snapshots";
        public static string CreateUri => "CreateSnapshot";
        public static string GetSnapshots => "GetFilteredSnapshots";

        public static string GetSnapshot(Guid id)
        {
            return $"GetSnapshot/{id}";
        }

        public static string DeleteSnapshot(Guid id)
        {
            return $"DeleteSnapshot/{id}";
        }
    }
}
