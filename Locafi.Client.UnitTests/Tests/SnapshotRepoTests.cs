using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Contract.Services;
using Locafi.Client.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests
{
    [TestClass]
    public class SnapshotRepoTests
    {
        private ISnapshotRepo _snapshotRepo;
        private IPlaceRepo _placeRepo;
        private IUserRepo _userRepo;

        [TestInitialize]
        public void Initialize()
        {
            _snapshotRepo = WebRepoContainer.SnapshotRepo;
            _placeRepo = WebRepoContainer.PlaceRepo;
            _userRepo = WebRepoContainer.UserRepo;
        }
        [TestMethod]
        public async Task CreateSnapshot() // failing due to non-hexadecimal tag numbers. mark to fix soon
        {
            var newSnap = await CreateRandomSnapshot();
            var result = await _snapshotRepo.PostSnapshot(newSnap);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result,typeof(SnapshotDto));
            Assert.IsTrue(result.Tags.Count == newSnap.Tags.Count);
            foreach (var tag in result.Tags)
            {
                Assert.IsTrue(newSnap.Tags.Contains(tag));
            }
            Assert.IsNotNull(result.Items);
            Assert.AreEqual(newSnap.PlaceId,result.PlaceId);
            Assert.AreEqual(newSnap.UserId, result.PlaceId);
        }

        [TestMethod]
        public async Task GetAllSnapshots() 
        {
            var snaps = await _snapshotRepo.GetAllSnapshots();
            Assert.IsInstanceOfType(snaps,typeof(IEnumerable<SnapshotDto>));
        }


        private async Task<SnapshotDto> CreateRandomSnapshot()
        {
            var ran = new Random();
            var places = await _placeRepo.GetAllPlaces();
            var place = places[ran.Next(places.Count - 1)];
            var users = await _userRepo.GetAllUsers();
            var user = users[ran.Next(users.Count - 1)];
            
            var name = Guid.NewGuid().ToString();
            var tags = GenerateRandomTags();
            var snap = new SnapshotDto
            {
                StartTimeUtc = DateTime.UtcNow.Subtract(new TimeSpan(0, 1, 0)),
                EndTimeUtc = DateTime.UtcNow,
                Name = name,
                Tags = tags,
                PlaceId = place.Id.ToString(),
                UserId = user.Id.ToString()
            };
            return snap;
        }

        private IList<SnapshotDtoTag> GenerateRandomTags()
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

        private SnapshotDtoTag GenerateRandomTag()
        {
            return new SnapshotDtoTag
            {
                TagNumber = Guid.NewGuid().ToString()
            };
        }
    }
}
