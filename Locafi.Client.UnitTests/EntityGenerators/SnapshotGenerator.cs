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
        public static AddSnapshotDto CreateRandomSnapshotForUpload(Guid placeId, int number = -1)
        {
            var ran = new Random();
            var count = number <= 0 ? ran.Next(50) : number;
            var name = Guid.NewGuid().ToString();
            var tags = GenerateRandomTags(count);
            var snap = new AddSnapshotDto(placeId)
            {
                StartTimeUtc = DateTime.UtcNow.Subtract(new TimeSpan(0, 1, 0)),
                EndTimeUtc = DateTime.UtcNow,
                Name = name,
                Tags = tags
            };
            return snap;
        }

        private static IList<SnapshotTagDto> GenerateRandomTags(int number)
        {

            var tags = new List<SnapshotTagDto>();
            for (var n = 0; n < number; n++)
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
