using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Devices;
using Locafi.Client.Model.Enums;
using Locafi.Client.UnitTests.EntityGenerators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Locafi.Client.Model.Query.Builder;

namespace Locafi.Client.UnitTests.Tests.Anthony
{
    [TestClass]
    public class DeviceRepoTests
    {
        private IDeviceRepo _deviceRepoAsPortal;
        private IDeviceRepo _deviceRepoAsUser;
        private IPlaceRepo _placeRepo;
        private IPersonRepo _personRepo;

        [TestInitialize]
        public void Initialize()
        {
            _deviceRepoAsPortal = WebRepoAsAuthorisedPortalContainer.DeviceRepo;
            _deviceRepoAsUser = WebRepoContainer.DeviceRepo;
            _placeRepo = WebRepoContainer.PlaceRepo;
            _personRepo = WebRepoContainer.PersonRepo;
        }
        
        //Peripheral Device Test Methods
        [TestMethod]
        public async Task PeripheralDevice_GetAll()
        {
            var devices = await _deviceRepoAsUser.QueryDevices();
            Assert.IsNotNull(devices);
            Assert.IsInstanceOfType(devices.Items, typeof(IEnumerable<PeripheralDeviceSummaryDto>));
            Assert.IsTrue(devices.Items.Count() > 0);

            var query = QueryBuilder<PeripheralDeviceSummaryDto>.NewQuery(d => d.DeviceType, PeripheralDeviceType.SpeedwayR420, Model.Query.ComparisonOperator.Equals)
                .Build();
            devices = await _deviceRepoAsUser.QueryDevices(query);
            Assert.IsNotNull(devices);
            Assert.IsInstanceOfType(devices.Items, typeof(IEnumerable<PeripheralDeviceSummaryDto>));
            Assert.IsTrue(devices.Items.Count() > 0);
        }

        [TestMethod]
        public async Task PeripheralDevice_GetByIdAsUser()
        {
            var devices = await _deviceRepoAsUser.QueryDevices();
            var device = await _deviceRepoAsUser.GetDevice(devices.First().Id);
            Assert.IsNotNull(device);
            Assert.IsInstanceOfType(device, typeof(PeripheralDeviceDetailDto));
        }

        [TestMethod]
        public async Task PeripheralDevice_GetByIdAsPortal()
        {
            var devices = await _deviceRepoAsUser.QueryDevices();
            var device = await _deviceRepoAsPortal.GetDevice(devices.First().Id);
            Assert.IsNotNull(device);
            Assert.IsInstanceOfType(device, typeof(PeripheralDeviceDetailDto));
        }

        //RFID Reader Test Methods
        [TestMethod]
        public async Task RfidReader_GetAll()
        {
            var readers = await _deviceRepoAsUser.QueryReaders();
            Assert.IsNotNull(readers);
            Assert.IsInstanceOfType(readers, typeof(IEnumerable<RfidReaderSummaryDto>));
            Assert.IsTrue(readers.Count > 0); //because we cant add readers at the moment
        }

        [TestMethod]
        public async Task RfidReader_GetBySerialAsUser()
        {
            var readerSerial = (await _deviceRepoAsUser.QueryReaders()).FirstOrDefault(r => !String.IsNullOrEmpty(r.SerialNumber)).SerialNumber;
            var reader = await _deviceRepoAsUser.GetReader(readerSerial);
            Assert.IsNotNull(reader);
            Assert.IsTrue(reader.SerialNumber == readerSerial);
        }

        [TestMethod]
        public async Task RfidReader_GetByIdAsUser()
        {
            var readerId = (await _deviceRepoAsUser.QueryReaders()).FirstOrDefault().Id;
            var readerTest = await _deviceRepoAsUser.GetReader(readerId);
            Assert.IsNotNull(readerTest);
            Assert.IsTrue(readerId == readerTest.Id);
        }

        [TestMethod]
        public async Task RfidReader_GetBySerialAsPortal()
        {
            var readerSerial = (await _deviceRepoAsUser.QueryReaders()).FirstOrDefault(r => !String.IsNullOrEmpty(r.SerialNumber)).SerialNumber;
            var reader = await _deviceRepoAsPortal.GetReader(readerSerial);
            Assert.IsNotNull(reader);
            Assert.IsTrue(reader.SerialNumber == readerSerial);
        }

        [TestMethod]
        public async Task RfidReader_GetByIdAsPortal()
        {
            var readerId = (await _deviceRepoAsUser.QueryReaders()).FirstOrDefault().Id;
            var readerTest = await _deviceRepoAsPortal.GetReader(readerId);
            Assert.IsNotNull(readerTest);
            Assert.IsTrue(readerId == readerTest.Id);
        }

        [TestMethod]
        public async Task RfidReader_ClusterTagsOnly()
        {
            var cluster = await GenerateRandomCluster();
            var result = await _deviceRepoAsPortal.ProcessCluster(cluster);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ClusterResponseDto));
        }

        [TestMethod]
        public async Task RfidReader_CreateRfidReader()
        {
            var addReader = DeviceGenerator.CreateRandomRfidReader();
            var newReader = await _deviceRepoAsUser.CreateReader(addReader);
            Assert.IsNotNull(newReader);
            Assert.IsInstanceOfType(newReader, typeof(RfidReaderDetailDto));
            await _deviceRepoAsUser.DeleteReader(newReader.Id);
        }

        [TestMethod]
        public async Task RfidReader_DeleteRfidReader()
        {
            try
            {
                var addReader = DeviceGenerator.CreateRandomRfidReader();
                var newReader = await _deviceRepoAsUser.CreateReader(addReader);
                await _deviceRepoAsUser.DeleteReader(newReader.Id);
                Assert.IsTrue(true);
            }
            catch
            {
                Assert.Fail();
            }
            
        }

        [TestMethod]
        public async Task PeripheralDevice_CreateDevice()
        {
            var addDevice = DeviceGenerator.CreateRandomPeripheralDevice();
            var newDevice = await _deviceRepoAsUser.CreateDevice(addDevice);
            Assert.IsNotNull(newDevice);
            Assert.IsInstanceOfType(newDevice, typeof(PeripheralDeviceDetailDto));
            await _deviceRepoAsUser.DeleteDevice(newDevice.Id);
        }

        [TestMethod]
        public async Task PeripheralDevice_DeleteDevice()
        {
            try
            {
                var addDevice = DeviceGenerator.CreateRandomPeripheralDevice();
                var newDevice = await _deviceRepoAsUser.CreateDevice(addDevice);
                await _deviceRepoAsUser.DeleteDevice(newDevice.Id);
                Assert.IsTrue(true);
            }
            catch
            {
                Assert.Fail();
            }
        }

        #region Private Methods        
        
        private async Task<ClusterDto> GenerateRandomCluster()
        {
            var ran = new Random();
            var places = await _placeRepo.QueryPlaces();
            var place = places.Items.ElementAt(ran.Next(places.Items.Count() - 1));

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
            var persons = await _personRepo.QueryPersons();
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
