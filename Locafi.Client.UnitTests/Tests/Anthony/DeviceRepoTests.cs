using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Devices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests.Anthony
{
    [TestClass]
    public class DeviceRepoTests
    {
        private IDeviceRepo _deviceRepoAsReader;
        private IDeviceRepo _deviceRepoAsUser;
        private IPlaceRepo _placeRepo;
        private IPersonRepo _personRepo;

        [TestInitialize]
        public void Initialize()
        {
            _deviceRepoAsReader = WebRepoAsAuthorisedReaderContainer.DeviceRepo;
            _deviceRepoAsUser = WebRepoContainer.DeviceRepo;
            _placeRepo = WebRepoContainer.PlaceRepo;
            _personRepo = WebRepoContainer.PersonRepo;
        }
        //Peripheral Device Test Methods

        //RFID Reader Test Methods
        [TestMethod]
        public async Task RfidReader_GetAll()
        {
            var readers = await _deviceRepoAsUser.GetReaders();
            Assert.IsNotNull(readers);
            Assert.IsInstanceOfType(readers, typeof(IEnumerable<RfidReaderSummaryDto>));
            Assert.IsTrue(readers.Count > 0); //because we cant add readers at the moment
        }

        [TestMethod]
        public async Task RfidReader_GetBySerialAsUser()
        {
            var reader = await _deviceRepoAsUser.GetReader(StringConstants.ReaderUserName);
            Assert.IsNotNull(reader);
        }

        [TestMethod]
        public async Task RfidReader_GetByIdAsUser()
        {
            var readerOriginal = await _deviceRepoAsUser.GetReader(StringConstants.ReaderUserName);
            var readerTest = await _deviceRepoAsUser.GetReader(readerOriginal.Id);
            Assert.IsTrue(readerOriginal.Id == readerTest.Id);
        }

        [TestMethod]
        public async Task RfidReader_GetBySerialAsReader()
        {
            var reader = await _deviceRepoAsReader.GetReader(StringConstants.ReaderUserName);
            Assert.IsNotNull(reader);
        }

        [TestMethod]
        public async Task RfidReader_GetByIdAsReader()
        {
            var readerOriginal = await _deviceRepoAsReader.GetReader(StringConstants.ReaderUserName);
            var readerTest = await _deviceRepoAsReader.GetReader(readerOriginal.Id);
            Assert.IsTrue(readerOriginal.Id == readerTest.Id);
        }

        [TestMethod]
        public async Task RfidReader_ClusterTagsOnly()
        {
            var cluster = await GenerateRandomCluster();
            var result = await _deviceRepoAsReader.ProcessCluster(cluster);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ClusterResponseDto));
        }

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

        private async Task AddNewPersonTag(ClusterDto cluster)
        {
            var persons = await _personRepo.GetAllPersons();
            foreach (var person in persons)
            {
                if (!string.IsNullOrEmpty(person.TagNumber) && !cluster.Tags.Any(t => string.Equals(t.TagNumber, person.TagNumber)))
                {
                    cluster.Tags.Add(new ClusterTagDto { TagNumber = person.TagNumber });
                    break;
                }
            }
        }
    }
}
