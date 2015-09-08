using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Snapshots;

namespace Locafi.Client.UnitTests.EntityGenerators
{
    public static class SnapshotGenerator
    {
        public static AddSnapshotDto CreateRandomSnapshotForUpload(Guid placeId)
        {
            var ran = new Random();

            var name = Guid.NewGuid().ToString();
            var tags = GenerateRandomTags();
            var snap = new AddSnapshotDto(placeId)
            {
                StartTimeUtc = DateTime.UtcNow.Subtract(new TimeSpan(0, 1, 0)),
                EndTimeUtc = DateTime.UtcNow,
                Name = name,
                Tags = tags
            };
            return snap;
        }

        private static IList<SnapshotTagDto> GenerateRandomTags()
        {
            var ran = new Random();
            var tags = new List<SnapshotTagDto>();
            var numTags = ran.Next(50);
            for (var n = 0; n < numTags; n++)
            {
                tags.Add(GenerateRandomTag());
            }
            return tags;
        }

        private static SnapshotTagDto GenerateRandomTag()
        {
            return new SnapshotTagDto
            {
                TagNumber = Guid.NewGuid().ToString()
            };
        }
    }
}
