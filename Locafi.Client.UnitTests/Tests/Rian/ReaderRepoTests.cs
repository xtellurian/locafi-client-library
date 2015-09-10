using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Reader;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests.Rian
{
    [TestClass]
    public class ReaderRepoTests
    {
        private IReaderRepo _readerRepo;
        private IPlaceRepo _placeRepo;
        private IPersonRepo _personRepo;

        [TestInitialize]
        public void Initialise()
        {
            _readerRepo = WebRepoContainer.ReaderRepo;
            _placeRepo = WebRepoContainer.PlaceRepo;
            _personRepo = WebRepoContainer.PersonRepo;
        }

        [TestMethod]
        public async Task Reader_GetAll()
        {
            var readers = await _readerRepo.GetReaders();
            Assert.IsNotNull(readers);
            Assert.IsInstanceOfType(readers,typeof(IEnumerable<ReaderSummaryDto>));
            Assert.IsTrue(readers.Count > 0); //because we cant add readers at the moment
        }

  //      [TestMethod]
        public async Task Reader_GetDetail()
        {
            var readers = await _readerRepo.GetReaders();
            Assert.IsNotNull(readers);
            Assert.IsInstanceOfType(readers, typeof(IEnumerable<ReaderSummaryDto>));

            foreach (var r in readers)
            {
                var reader = await _readerRepo.GetReaderById(r.Id);
                Assert.IsNotNull(reader);
                Assert.IsInstanceOfType(reader,typeof(ReaderDetailDto));
                Assert.AreEqual(reader,r);
            }
        }
    //    [TestMethod]
        public async Task Reader_ClusterTagsOnly()
        {
            var cluster = await GenerateRandomCluster();
            var result = await _readerRepo.ProcessCluster(cluster);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result,typeof(ClusterResponseDto));
        }


        #region Private Methods

        private async Task<ClusterDto> GenerateRandomCluster()
        {
            var ran = new Random();
            var places = await _placeRepo.GetAllPlaces();
            var place = places[ran.Next(places.Count - 1)];

            var cluster = new ClusterDto
            {
                PlaceId = place.Id,
                Tags = GenerateRandomClusterTags(),
                TimeStamp = DateTime.UtcNow
            };
            return cluster;
        }

        private async Task AddNewPersonTag(ClusterDto cluster)
        {
            var persons = await _personRepo.GetAllPersons();
            foreach (var person in persons)
            {
                if (!string.IsNullOrEmpty(person.TagNumber) && !cluster.Tags.Any(t => string.Equals(t.TagNumber, person.TagNumber)))
                {
                    cluster.Tags.Add(new ClusterTagDto {TagNumber = person.TagNumber});
                    break;
                }
            }

        }

        private IList<ClusterTagDto> GenerateRandomClusterTags()
        {
            var ran = new Random();
            var tags = new List<ClusterTagDto>();
            var numTags = ran.Next(100);// max 100 tags
            for (var i = 0; i < numTags; i++)
            {
                tags.Add(new ClusterTagDto
                {
                    TagNumber = Guid.NewGuid().ToString()
                });
            }
            return tags;
        }

        #endregion

    }
}
