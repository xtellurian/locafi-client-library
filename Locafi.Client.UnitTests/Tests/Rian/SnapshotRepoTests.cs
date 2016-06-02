﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Places;
using Locafi.Client.Model.Dto.Snapshots;
using Locafi.Client.UnitTests.EntityGenerators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Locafi.Client.Model;

namespace Locafi.Client.UnitTests.Tests.Rian
{
    [TestClass]
    public class SnapshotRepoTests
    {
        private ISnapshotRepo _snapshotRepo;
        private IPlaceRepo _placeRepo;
        private IList<Guid> _toCleanup;
        private ISkuRepo _skuRepo;

        [TestInitialize]
        public void Initialize()
        {
            _snapshotRepo = WebRepoContainer.SnapshotRepo;
            _placeRepo = WebRepoContainer.PlaceRepo;
            _toCleanup = new List<Guid>();
            _skuRepo = WebRepoContainer.SkuRepo;
        }

        [TestMethod]
        public async Task Snapshot_Create_RandomTags() 
        {
            var place = await GetRandomPlace();
            var newSnap = SnapshotGenerator.CreateRandomSnapshotForUpload(place.Id,5000);
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
        public async Task Snapshot_Create_NewGtinTags()
        {
            var place = await GetRandomPlace();
            var newSnap = await SnapshotGenerator.CreateNewGtinSnapshotForUpload(place.Id, 5000);
            var result = await _snapshotRepo.CreateSnapshot(newSnap);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(SnapshotDetailDto));
            Assert.IsTrue(result.Tags.Count == newSnap.Tags.Count);
            foreach (var tag in result.Tags)
            {
                Assert.IsTrue(newSnap.Tags.Contains(tag));
            }
            Assert.IsNotNull(result.Items);
            Assert.AreEqual(newSnap.PlaceId, result.PlaceId);

            _toCleanup.Add(result.Id); // add to cleanup
        }

        [TestMethod]
        public async Task Snapshot_Create_ExistingGtinTags()
        {
            var place = await GetRandomPlace();
            var newSnap = await SnapshotGenerator.CreateExistingGtinSnapshotForUpload(place.Id, 5000);
            var result = await _snapshotRepo.CreateSnapshot(newSnap);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(SnapshotDetailDto));
            Assert.IsTrue(result.Tags.Count == newSnap.Tags.Count);
            foreach (var tag in result.Tags)
            {
                Assert.IsTrue(newSnap.Tags.Contains(tag));
            }
            Assert.IsNotNull(result.Items);
            Assert.AreEqual(newSnap.PlaceId, result.PlaceId);

            _toCleanup.Add(result.Id); // add to cleanup
        }

        [TestMethod]
        public async Task Snapshot_GetAll() 
        {
            var place = await GetRandomPlace();
            var newSnap = SnapshotGenerator.CreateRandomSnapshotForUpload(place.Id);
            var result = await _snapshotRepo.CreateSnapshot(newSnap);
            _toCleanup.Add(result.Id);

            var snaps = await _snapshotRepo.QuerySnapshots();
            Assert.IsNotNull(snaps);
            Assert.IsInstanceOfType(snaps,typeof(PageResult<SnapshotSummaryDto>));
            Assert.IsTrue(snaps.Items.Contains(result));
        }

   //     [TestMethod]
        public async Task Snapshot_GetById() // assumes some exist already
        {
            var snaps = await _snapshotRepo.QuerySnapshots();
            foreach (var snap in snaps.Items)
            {
                var result = await _snapshotRepo.GetSnapshot(snap.Id);
                Assert.IsNotNull(result.Tags, "result.Tags != null");
                Assert.IsNotNull(result.Items, "result.Items != null");
                Assert.AreEqual(snap, result);
            }
        }
  //      [TestMethod]
        public async Task Snapshot_Delete()
        {
            // create snapshot
            var place = await GetRandomPlace();
            var newSnap = SnapshotGenerator.CreateRandomSnapshotForUpload(place.Id);
            var result = await _snapshotRepo.CreateSnapshot(newSnap);
            Assert.IsNotNull(result);
            var allSnaps = await _snapshotRepo.QuerySnapshots();
            Assert.IsTrue(allSnaps.Items.Contains(result)); // make sure it was created

            await _snapshotRepo.Delete(result.Id);
            allSnaps = await _snapshotRepo.QuerySnapshots();
            Assert.IsFalse(allSnaps.Items.Contains(result)); // make sure it was deleted

        }

      
        private async Task<PlaceSummaryDto> GetRandomPlace()
        {
            var ran = new Random();
            var places = await _placeRepo.QueryPlaces();
            var place = places.Items.ElementAt(ran.Next(places.Items.Count() - 1));
            return place;
        }

    }
}
