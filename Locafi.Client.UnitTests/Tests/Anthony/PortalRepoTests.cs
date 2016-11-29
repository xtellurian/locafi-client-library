using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
//using Locafi.Client.Model.Dto.Devices;
//using Locafi.Client.Model.Dto.PortalRules;
using Locafi.Client.Model.Dto.Portals;
using Locafi.Client.Model.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests.Anthony
{
//    [TestClass]
//    public class PortalRepoTests
//    {
//        private IPortalRepo _portalRepoAsPortal;
//        private IPortalRepo _portalRepo;
//        private IPlaceRepo _placeRepo;
//        private IDeviceRepo _deviceRepo;

//        [TestInitialize]
//        public void Initialize()
//        {
//            _portalRepoAsPortal = WebRepoAgentContainer.PortalRepo;
//            _portalRepo = WebRepoContainer.PortalRepo;
//            _placeRepo = WebRepoContainer.PlaceRepo;
//            _deviceRepo = WebRepoContainer.DeviceRepo;
//        }

//        [TestMethod]
//        public async Task Portal_GetPortals()
//        {
//            var portals = await _portalRepo.GetPortals();
//            Assert.IsNotNull(portals);
//            Assert.IsInstanceOfType(portals, typeof (IEnumerable<PortalSummaryDto>));
//            Assert.IsTrue(portals.Count > 0);
//        }

//        [TestMethod]
//        public async Task Portal_GetPortalByIdAsUser()
//        {
//            var portals = await _portalRepo.GetPortals();
//            var portal = await _portalRepo.GetPortal(portals.First().Id);
//            Assert.IsNotNull(portal);
//            Assert.IsInstanceOfType(portal, typeof (PortalDetailDto));
//        }

//        [TestMethod]
//        public async Task Portal_GetPortalByIdAsPortal()
//        {
//            var portals = await _portalRepo.GetPortals();
//            var portalId = portals.FirstOrDefault(t => t.SerialNumber == StringConstants.PortalUsername).Id;
//            var portal = await _portalRepoAsPortal.GetPortal(portalId); //fails as it's unauthorised
//            Assert.IsNotNull(portal);
//            Assert.IsInstanceOfType(portal, typeof (PortalDetailDto));
//        }

//        [TestMethod]
//        public async Task Portal_GetPortalBySerialAsPortal()
//        {
//            var portal = await _portalRepoAsPortal.GetPortal(StringConstants.PortalUsername);
//            Assert.IsNotNull(portal);
//            Assert.IsInstanceOfType(portal, typeof (PortalDetailDto));
//        }

//        [TestMethod]
//        public async Task Portal_CreatePortal()
//        {
//            var newPortal = await _portalRepo.CreatePortal(CreateRandomPortal());
//            Assert.IsNotNull(newPortal);
//            Assert.IsInstanceOfType(newPortal, typeof (PortalDetailDto));
//            await _portalRepo.DeletePortal(newPortal.Id);
//        }

//        private AddPortalDto CreateRandomPortal()
//        {
//            return new AddPortalDto
//            {
//                MaxPeripheralDevices = 3,
//                MaxRfidReaders = 3,
//                Name = "CatPortal",
//                SerialNumber = Guid.NewGuid().ToString()
//            };
//        }

//        [TestMethod]
//        public async Task Portal_UpdatePortal()
//        {
//            var newPortal = await _portalRepo.CreatePortal(CreateRandomPortal());
//            var update = new UpdatePortalDto
//            {
//                Id = newPortal.Id,
//                MaxPeripheralDevices = newPortal.MaxPeripheralDevices,
//                MaxRfidReaders = 7,
//                Name = newPortal.Name,
//                PeripheralDevices = new List<Guid>(),
//                RfidReaders = new List<Guid>(),
//                SerialNumber = newPortal.SerialNumber
//            };
//            var updatedPortal = await _portalRepo.UpdatePortal(update);
//            Assert.IsNotNull(updatedPortal);
//            Assert.IsInstanceOfType(updatedPortal, typeof (PortalDetailDto));
//            Assert.IsTrue(updatedPortal.MaxRfidReaders == 7);
//            await _portalRepo.DeletePortal(updatedPortal.Id);
//        }

//        [TestMethod]
//        public async Task Portal_DeletePortal()
//        {
//            try
//            {
//                var newPortal = await _portalRepo.CreatePortal(CreateRandomPortal());
//                await _portalRepo.DeletePortal(newPortal.Id);
//                Assert.IsTrue(true);
//            }
//            catch
//            {
//                Assert.Fail();
//            }
//        }

//        [TestMethod]
//        public async Task Portal_GetPortalRules()
//        {
//            var portalRules = await _portalRepo.GetPortalRules();
//            Assert.IsNotNull(portalRules);
//            Assert.IsInstanceOfType(portalRules, typeof (IEnumerable<PortalRuleSummaryDto>));
//            Assert.IsTrue(portalRules.Count > 0);
//        }

//        [TestMethod]
//        public async Task Portal_GetPortalRuleByIdAsUser()
//        {
//            var portalRules = await _portalRepo.GetPortalRules();
//            var portalRule = await _portalRepo.GetPortalRule(portalRules.FirstOrDefault().Id);
//            Assert.IsNotNull(portalRule);
//            Assert.IsInstanceOfType(portalRule, typeof (PortalRuleDetailDto));
//        }

//        [TestMethod]
//        public async Task Portal_GetPortalRuleByIdAsPortal()
//        {
//            var portalRules = await _portalRepo.GetPortalRules();
//            var portalRule = await _portalRepoAsPortal.GetPortalRule(portalRules.FirstOrDefault().Id);
//            Assert.IsNotNull(portalRule);
//            Assert.IsInstanceOfType(portalRule, typeof (PortalRuleDetailDto));
//        }

//        [TestMethod]
//        public async Task Portal_GetPortalRulesForSpecificPortalAsUser()
//        {
//            var portals = await _portalRepo.GetPortals();
//            var portalRules = await _portalRepo.GetRulesForPortal(portals.FirstOrDefault(t => !String.IsNullOrEmpty(t.SerialNumber)).Id);
//            Assert.IsNotNull(portalRules);
//            Assert.IsInstanceOfType(portalRules, typeof (IEnumerable<PortalRuleDetailDto>));
//        }

//        [TestMethod]
//        public async Task Portal_GetPortalRulesForSpecificPortalAsPortal()
//        {
//            var portals = await _portalRepo.GetPortals();
//            var portalRules =
//                await
//                    _portalRepoAsPortal.GetRulesForPortal(
//                        portals.FirstOrDefault(t => !String.IsNullOrEmpty(t.SerialNumber)).Id);
//            Assert.IsNotNull(portalRules);
//            Assert.IsInstanceOfType(portalRules, typeof (IEnumerable<PortalRuleDetailDto>));
////            Assert.IsTrue(portalRules.Count > 0);
//        }

//        [TestMethod]
//        public async Task Portal_CreatePortalRule()
//        {
//            var addPortalRule = await CreateRandomPortalRule();
//            var newPortalRule = await _portalRepo.CreatePortalRule(addPortalRule);
//            Assert.IsNotNull(newPortalRule);
//            Assert.IsInstanceOfType(newPortalRule, typeof (PortalRuleDetailDto));
//            await _portalRepo.DeletePortalRule(newPortalRule.Id);
//        }

//        private async Task<AddPortalRuleDto> CreateRandomPortalRule()
//        {
//            var portalId = (await _portalRepo.GetPortals()).FirstOrDefault().Id;
//            var placeInId = (await _placeRepo.QueryPlaces()).FirstOrDefault().Id;

//            return new AddPortalRuleDto
//            {
//                RfidPortalId = portalId,
//                Antennas = new List<Guid>(),
//                Name = "Cat Portal Rule",
//                PlaceInId = placeInId,
//                PlaceOutId = null,
//                RuleType = PortalRuleType.AntennaEvents,
//                SensorInId = null,
//                SensorOutId = null,
//                Timeout = 3000
//            };
//        }

//        [TestMethod]
//        public async Task Portal_UpdatePortalRule()
//        {
//            var newPortalRule = await _portalRepo.CreatePortalRule(await CreateRandomPortalRule());
//            var placeInId = (await _placeRepo.QueryPlaces()).FirstOrDefault().Id;
//            var update = new UpdatePortalRuleDto
//            {
//                Antennas = new List<Guid>(),
//                Name = "Cat Portal Rule",
//                PlaceInId = placeInId,
//                PlaceOutId = null,
//                RuleType = PortalRuleType.AntennaEvents,
//                SensorInId = null,
//                SensorOutId = null,
//                Timeout = 5000,
//                Id = newPortalRule.Id
//            };
//            var updatedPortalRule = await _portalRepo.UpdatePortalRule(update);
//            Assert.IsNotNull(updatedPortalRule);
//            Assert.IsInstanceOfType(updatedPortalRule, typeof (PortalRuleDetailDto));
//            Assert.IsTrue(updatedPortalRule.Timeout == update.Timeout);
//            await _portalRepo.DeletePortalRule(updatedPortalRule.Id);
//        }

//        [TestMethod]
//        public async Task Portal_DeletePortalRule()
//        {
//            try
//            {
//                var newPortalRule = await _portalRepo.CreatePortalRule(await CreateRandomPortalRule());
//                await _portalRepo.DeletePortalRule(newPortalRule.Id);
//                Assert.IsTrue(true);
//            }
//            catch
//            {
//                Assert.Fail();
//            }
//        }

//        [TestMethod]
//        public async Task Portal_GetPortalStatus()
//        {
//            var portalId = (await _portalRepo.GetPortals()).FirstOrDefault().Id;
//            var status = await _portalRepo.GetPortalStatus(portalId);
//            Assert.IsNotNull(status);
//            Assert.IsInstanceOfType(status, typeof(PortalStatusDto));
//        }

//        [TestMethod]
//        public async Task Portal_UpdatePortalStatus()
//        {
//            var portalId = (await _portalRepo.GetPortals()).FirstOrDefault().Id;
//            var update = await CreateRandomPortalStatusUpdate(portalId);
//            var result = await _portalRepoAsPortal.UpdatePortalStatus(update);
//            Assert.IsNotNull(result);
//            Assert.IsInstanceOfType(result, typeof(PortalStatusDto));
//        }

//        private async Task<UpdatePortalStatusDto> CreateRandomPortalStatusUpdate(Guid portalId)
//        {
//            var portal = await _portalRepo.GetPortal(portalId);
//            var update = new UpdatePortalStatusDto
//            {
//                Id = portal.Id,
//                RfidReaderStatuses = new List<RfidReaderStatusDto>(),
//                Status = RfidPortalStatus.Online,
//                TimeStamp = DateTime.Now
//            };
//            foreach (var reader in portal.RfidReaders)
//            {
//                update.RfidReaderStatuses.Add(new RfidReaderStatusDto
//                {
//                    AntennaStatuses = new List<RfidReaderAntennaStatusDto>(),
//                    Status = DeviceStatus.Offline,
//                    Id = reader.Id
//                });
//                foreach (var antenna in reader.Antennas)
//                {
//                    update.RfidReaderStatuses.First(r => r.Id == reader.Id).AntennaStatuses.Add(new RfidReaderAntennaStatusDto
//                    {
//                        Id = antenna.Id,
//                        Status = RfidReaderAntennaStatus.Idle
//                    });
//                }
//            }
//            return update;
//        }

//        [TestMethod]
//        public async Task Portal_UpdatePortalHeartbeat()
//        {
//            var portalId = (await _portalRepo.GetPortals()).FirstOrDefault().Id;
//            var heartbeat = new PortalHeartbeatDto {RfidPortalId = portalId, RfidReaderTemperatures = new List<RfidReaderTemperatureDto>()};
//            try
//            {
//                await _portalRepoAsPortal.UpdatePortalHeartbeat(heartbeat);
//                Assert.IsTrue(true);
//            }
//            catch (Exception e)
//            {
//                Assert.Fail(e.Message);
//            }
            
//        }
//}
}
