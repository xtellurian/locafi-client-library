using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Locafi.Client.Crypto;
using Locafi.Client.Model.Dto.Ble;
using Locafi.Client.Repo;
using Locafi.Client.UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests.Rian.Ble
{
    [TestClass]
    public class BleRepoTests
    {
        [TestMethod]
        public async Task BleRepoUnitTest()
        {
            var baseUrl = "hello";
            var sydneyGeoHash = "r652uhdug";
            var mockTransferer = new MockHttpTransferrer(true);

            var repo = new BleDetectionRepo(mockTransferer, new Sha256HashService(),baseUrl,"app", "12341234");
            var tagNumber = Guid.NewGuid().ToString();
            var detections = new List<BleDetectionBase>()
            {
                new SensorBleDetection()
                {
                    DetectedBy = "Rian",
                    TagNumber = tagNumber,
                    Timestamp = DateTime.Now,
                    LocationGeoHash = sydneyGeoHash
                }
            };
            await repo.UploadDetections(detections);

            Assert.IsTrue(mockTransferer.HttpCalls.Keys.Any(c => c.Contains(baseUrl)));
        }
    }
}
