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
        private IDeviceRepo _deviceRepoAsDevice;
        private IDeviceRepo _deviceRepoAsUser;
        private IPlaceRepo _placeRepo;
        private IPersonRepo _personRepo;

        [TestInitialize]
        public void Initialize()
        {
            _deviceRepoAsDevice = WebRepoAsAuthorisedReaderContainer.DeviceRepo;
            _deviceRepoAsUser = WebRepoContainer.DeviceRepo;
            _placeRepo = WebRepoContainer.PlaceRepo;
            _personRepo = WebRepoContainer.PersonRepo;
        }
        //TODO: Write tests for creation and deletion of peripheral devices, readers
        //Peripheral Device Test Methods
        [TestMethod]
        public async Task PeripheralDevice_GetAll()
        {
            var devices = await _deviceRepoAsUser.GetDevices();
            Assert.IsNotNull(devices);
            Assert.IsInstanceOfType(devices, typeof(IEnumerable<PeripheralDeviceSummaryDto>));
            Assert.IsTrue(devices.Count > 0);
        }

        [TestMethod]
        public async Task PeripheralDevice_GetByIdAsUser()
        {
            var devices = await _deviceRepoAsUser.GetDevices();
            var device = await _deviceRepoAsUser.GetDevice(devices.First().Id);
            Assert.IsNotNull(device);
            Assert.IsInstanceOfType(device, typeof(PeripheralDeviceDetailDto));
        }

        [TestMethod]
        public async Task PeripheralDevice_GetByIdAsDevice()
        {
            var devices = await _deviceRepoAsUser.GetDevices();
            var device = await _deviceRepoAsDevice.GetDevice(devices.First().Id);
            Assert.IsNotNull(device);
            Assert.IsInstanceOfType(device, typeof(PeripheralDeviceDetailDto));
        }

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
        public async Task RfidReader_GetBySerialAsDevice()
        {
            var reader = await _deviceRepoAsDevice.GetReader(StringConstants.ReaderUserName);
            Assert.IsNotNull(reader);
        }

        [TestMethod]
        public async Task RfidReader_GetByIdAsDevice()
        {
            var readerOriginal = await _deviceRepoAsDevice.GetReader(StringConstants.ReaderUserName);
            var readerTest = await _deviceRepoAsDevice.GetReader(readerOriginal.Id);
            Assert.IsTrue(readerOriginal.Id == readerTest.Id);
        }

        [TestMethod]
        public async Task RfidReader_ClusterTagsOnly()
        {
            var cluster = await GenerateRandomCluster();
            var result = await _deviceRepoAsDevice.ProcessCluster(cluster);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ClusterResponseDto));
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
        #endregion
    }
}
