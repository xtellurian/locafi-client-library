using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Data;
using Locafi.Client.Model.Dto.Places;
using Locafi.Client.Model.Dto.Snapshots;
using Locafi.Client.UnitTests.EntityGenerators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests
{
    [TestClass]
    public class SnapshotRepoTests
    {
        private ISnapshotRepo _snapshotRepo;
        private IPlaceRepo _placeRepo;
        private IList<Guid> _toCleanup;

        [TestInitialize]
        public void Initialize()
        {
            _snapshotRepo = WebRepoContainer.SnapshotRepo;
            _placeRepo = WebRepoContainer.PlaceRepo;
            _toCleanup = new List<Guid>();
        }

        [TestMethod]
        public async Task Snapshot_Create() 
        {
            var place = await GetRandomPlace();
            var newSnap = SnapshotGenerator.CreateRandomSnapshotForUpload(place.Id);
            var result = await _snapshotRepo.CreateSnapshot(newSnap);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result,typeof(SnapshotDetailDto));
            Assert.IsTrue(result.Tags.Count == newSnap.Tags.Count);
            foreach (var tag in result.Tags)
            {
                Assert.IsTrue(newSnap.Tags.Contains(tag));
            }
            Assert.IsNotNull(result.Items);
            Assert.AreEqual(newSnap.PlaceId,result.PlaceId);

            _toCleanup.Add(result.Id); // add to cleanup
        }



        [TestMethod]
        public async Task Snapshot_GetAll() 
        {
            var place = await GetRandomPlace();
            var newSnap = SnapshotGenerator.CreateRandomSnapshotForUpload(place.Id);
            var result = await _snapshotRepo.CreateSnapshot(newSnap);
            _toCleanup.Add(result.Id);

            var snaps = await _snapshotRepo.GetAllSnapshots();
            Assert.IsNotNull(snaps);
            Assert.IsInstanceOfType(snaps,typeof(IEnumerable<SnapshotSummaryDto>));
            Assert.IsTrue(snaps.Contains(result));
        }

        [TestMethod]
        public async Task Snapshot_GetById() // assumes some exist already
        {
            var snaps = await _snapshotRepo.GetAllSnapshots();
            foreach (var snap in snaps)
            {
                var result = await _snapshotRepo.GetSnapshot(snap.Id);
                Assert.IsNotNull(result.Tags, "result.Tags != null");
                Assert.IsNotNull(result.Items, "result.Items != null");
                Assert.AreEqual(snap, result);
            }
        }
        [TestMethod]
        public async Task Snapshot_Delete()
        {
            // create snapshot
            var place = await GetRandomPlace();
            var newSnap = SnapshotGenerator.CreateRandomSnapshotForUpload(place.Id);
            var result = await _snapshotRepo.CreateSnapshot(newSnap);
            Assert.IsNotNull(result);
            var allSnaps = await _snapshotRepo.GetAllSnapshots();
            Assert.IsTrue(allSnaps.Contains(result)); // make sure it was created

            await _snapshotRepo.Delete(result.Id);
            allSnaps = await _snapshotRepo.GetAllSnapshots();
            Assert.IsFalse(allSnaps.Contains(result)); // make sure it was deleted

        }
        [TestCleanup]
        public async void Cleanup()
        {
            foreach (var id in _toCleanup)
            {
                await _snapshotRepo.Delete(id);
            }
        }
        private async Task<PlaceSummaryDto> GetRandomPlace()
        {
            var ran = new Random();
            var allPlaces = await _placeRepo.GetAllPlaces();
            var place = allPlaces[ran.Next(allPlaces.Count - 1)];
            return place;
        }

    }
}
