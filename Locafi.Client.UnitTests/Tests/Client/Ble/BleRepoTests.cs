using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using Locafi.Client.Crypto;
using Locafi.Client.Model.Dto.Ble;
using Locafi.Client.Repo;
using Locafi.Client.UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests.Ble
{
    [TestClass]
    public class BleRepoTests
    {
        [TestMethod]
        public async Task BleRepoUnitTest()
        {
            var baseUrl = "hello";
            
            var mockTransferer = new MockHttpTransferrer(true);

            var repo = new BleDetectionRepo(mockTransferer, new Sha256HashService(),baseUrl,"app", "12341234");
            
            var detections = GetSomeDetections();
            await repo.UploadDetections(detections);

            Assert.IsTrue(mockTransferer.HttpCalls.Keys.Any(c => c.Contains(baseUrl)));
        }

        private static List<BleDetectionBase> GetSomeDetections()
        {
            var tagNumber = Guid.NewGuid().ToString();
            var sydneyGeoHash = "r652uhdug";
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
            return detections;
        }

        [TestMethod]
        public async Task BleRepoIntegrationTest()
        {
            var goodBaseUrl = "http://localhost:48078";
            var appId = "4d53bce03ec34c0a911182d4c228ee6c";
            var secret = "A93reRTUJHsCuQSHR+L3GxqOJyDmQpCgps102ciuabc=";


            var repo = new BleDetectionRepo(goodBaseUrl,appId,secret);

            await repo.UploadDetections(GetSomeDetections());

            Assert.IsTrue(true); // if we dont get here, then an exception was thrown
        }

        [TestMethod]
        public async Task BleRepoLoadTest()
        {
            var goodBaseUrl = "http://localhost:48078";
            var appId = "4d53bce03ec34c0a911182d4c228ee6c";
            var secret = "A93reRTUJHsCuQSHR+L3GxqOJyDmQpCgps102ciuabc=";


            var repo = new BleDetectionRepo(goodBaseUrl, appId, secret);

            var detections = GetLotsOfDetections();

            await repo.UploadDetections(detections);
            
        }

        private List<BleDetectionBase> GetLotsOfDetections()
        {
            var startDate = DateTime.UtcNow.AddYears(-1);
            var ran = new Random(unchecked((int)DateTime.Now.Ticks));
            var numberOfDetections = ran.Next(1, 1000);
            var detectedBy = "Automated Test in Locafi Client";
            var result = new List<BleDetectionBase>();
            for (var i = numberOfDetections; i > 0; i--)
            {
                var tag = Guid.NewGuid().ToString();
                var time = startDate.AddDays(ran.Next(364)).AddSeconds(ran.Next(60*60*24));
                var det = new SensorBleDetection()
                {
                    DetectedBy = detectedBy,
                    TagNumber = tag,
                    Timestamp = time
                };
                if (RandomBool()) det.AccelerationInMetresPerSecond = ran.Next(1000);
                if (RandomBool()) det.LightInLumins = ran.Next(1000);
                if (RandomBool()) det.TemperatureInCelcius = ran.Next(50);
                result.Add(det);
             
            }
            return result;
        }

        private bool RandomBool()
        {
            var ran = new Random(DateTime.UtcNow.Millisecond);
            return ran.Next(100)%2 == 0;
        }
    }
}
