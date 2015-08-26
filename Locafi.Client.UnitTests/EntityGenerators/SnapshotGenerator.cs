using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Data;

namespace Locafi.Client.UnitTests.EntityGenerators
{
    public static class SnapshotGenerator
    {
        public static SnapshotDto CreateRandomSnapshot(string placeId, string userId)
        {
            var ran = new Random();

            var name = Guid.NewGuid().ToString();
            var tags = GenerateRandomTags();
            var snap = new SnapshotDto
            {
                StartTimeUtc = DateTime.UtcNow.Subtract(new TimeSpan(0, 1, 0)),
                EndTimeUtc = DateTime.UtcNow,
                Name = name,
                Tags = tags,
                PlaceId = placeId,
                UserId = userId
            };
            return snap;
        }

        private static IList<SnapshotDtoTag> GenerateRandomTags()
        {
            var ran = new Random();
            var tags = new List<SnapshotDtoTag>();
            var numTags = ran.Next(50);
            for (var n = 0; n < numTags; n++)
            {
                tags.Add(GenerateRandomTag());
            }
            return tags;
        }

        private static SnapshotDtoTag GenerateRandomTag()
        {
            return new SnapshotDtoTag
            {
                TagNumber = Guid.NewGuid().ToString()
            };
        }
    }
}
