using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Contract.Services;
using Locafi.Client.Data;
using Locafi.Client.Model.Dto.Places;
using Locafi.Client.UnitTests.EntityGenerators;
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
        public async Task Snapshot_Create() 
        {
            var place = await GetRandomPlace();
            var user = await GetRandomUser();
            var newSnap = SnapshotGenerator.CreateRandomSnapshot(place.Id.ToString(), user.Id.ToString());
            var result = await _snapshotRepo.CreateSnapshot(newSnap);
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
        public async Task Snapshot_GetAll() 
        {
            var snaps = await _snapshotRepo.GetAllSnapshots();
            Assert.IsInstanceOfType(snaps,typeof(IEnumerable<SnapshotDto>));
        }

        [TestMethod]
        public async Task Snapshot_GetById()
        {
            var snaps = await _snapshotRepo.GetAllSnapshots();
            foreach (var snap in snaps)
            {
                var result = await _snapshotRepo.GetSnapshot(snap.Id);
                Assert.IsNotNull(result.Tags);
                Assert.IsNotNull(result.Items);
                Assert.AreEqual(snap, result);
            }
        }

        private async Task<PlaceSummaryDto> GetRandomPlace()
        {
            var ran = new Random();
            var allPlaces = await _placeRepo.GetAllPlaces();
            var place = allPlaces[ran.Next(allPlaces.Count - 1)];
            return place;
        }

        private async Task<UserDto> GetRandomUser()
        {
            var ran = new Random();
            var allUsers = await _userRepo.GetAllUsers();
            var user = allUsers[ran.Next(allUsers.Count - 1)];
            return user;
        }

    }
}
