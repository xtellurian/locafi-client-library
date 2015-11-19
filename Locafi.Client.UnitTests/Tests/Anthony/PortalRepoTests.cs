using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Portal;
using Locafi.Client.Model.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests.Anthony
{
    [TestClass]
    public class PortalRepoTests
    {
        private IPortalRepo _portalRepoAsPortal;
        private IPortalRepo _portalRepo;
        private IPlaceRepo _placeRepo;

        [TestInitialize]
        public void Initialize()
        {
            _portalRepoAsPortal = WebRepoAsAuthorisedPortalContainer.PortalRepo;
            _portalRepo = WebRepoContainer.PortalRepo;
            _placeRepo = WebRepoContainer.PlaceRepo;
        }

        [TestMethod]
        public async Task Portal_GetPortals()
        {
            var portals = await _portalRepo.GetPortals();
            Assert.IsNotNull(portals);
            Assert.IsInstanceOfType(portals, typeof(IEnumerable<PortalSummaryDto>));
            Assert.IsTrue(portals.Count > 0);
        }

        [TestMethod]
        public async Task Portal_GetPortalByIdAsUser()
        {
            var portals = await _portalRepo.GetPortals();
            var portal = await _portalRepo.GetPortal(portals.First().Id);
            Assert.IsNotNull(portal);
            Assert.IsInstanceOfType(portal, typeof(PortalDetailDto));
        }

        [TestMethod]
        public async Task Portal_GetPortalByIdAsPortal()
        {
            var portals = await _portalRepo.GetPortals();
            var portalId = portals.FirstOrDefault(t => t.SerialNumber == StringConstants.PortalUsername).Id;
            var portal = await _portalRepoAsPortal.GetPortal(portalId); //fails as it's unauthorised
            Assert.IsNotNull(portal);
            Assert.IsInstanceOfType(portal, typeof(PortalDetailDto));
        }

        [TestMethod]
        public async Task Portal_CreatePortal()
        {
            var newPortal = await _portalRepo.CreatePortal(CreateRandomPortal());           
            Assert.IsNotNull(newPortal);
            Assert.IsInstanceOfType(newPortal, typeof(PortalDetailDto));
            await _portalRepo.DeletePortal(newPortal.Id);
        }

        private AddPortalDto CreateRandomPortal()
        {
            return new AddPortalDto
            {
                MaxPeripheralDevices = 3,
                MaxRfidReaders = 3,
                Name = "CatPortal",
                SerialNumber = "000000001"
            };
        }

        [TestMethod]
        public async Task Portal_UpdatePortal()
        {
            var newPortal = await _portalRepo.CreatePortal(CreateRandomPortal());
            var update = new UpdatePortalDto
            {
                Id = newPortal.Id,
                MaxPeripheralDevices = newPortal.MaxPeripheralDevices,
                MaxRfidReaders = 7,
                Name = newPortal.Name,
                PeripheralDevices = new List<Guid>(),
                RfidReaders = new List<Guid>(),
                SerialNumber = newPortal.SerialNumber
            };
            var updatedPortal = await _portalRepo.UpdatePortal(update);
            Assert.IsNotNull(updatedPortal);
            Assert.IsInstanceOfType(updatedPortal, typeof(PortalDetailDto));
            Assert.IsTrue(updatedPortal.MaxRfidReaders == 7);
            await _portalRepo.DeletePortal(updatedPortal.Id);
        }

        [TestMethod]
        public async Task Portal_DeletePortal()
        {
            var newPortal = await _portalRepo.CreatePortal(CreateRandomPortal());
            await _portalRepo.DeletePortal(newPortal.Id);
            var deletedPortal = await _portalRepo.GetPortal(newPortal.Id);
            Assert.IsNull(deletedPortal);            
        }

        [TestMethod]
        public async Task Portal_GetPortalRules()
        {
            var portalRules = await _portalRepo.GetPortalRules();
            Assert.IsNotNull(portalRules);
            Assert.IsInstanceOfType(portalRules, typeof(IEnumerable<PortalRuleSummaryDto>));
            Assert.IsTrue(portalRules.Count > 0);
        }

        [TestMethod]
        public async Task Portal_GetPortalRuleByIdAsUser()
        {
            var portalRules = await _portalRepo.GetPortalRules();
            var portalRule = await _portalRepo.GetPortalRule(portalRules.FirstOrDefault().Id);
            Assert.IsNotNull(portalRule);
            Assert.IsInstanceOfType(portalRule, typeof(PortalRuleDetailDto));
        }

        [TestMethod]
        public async Task Portal_GetPortalRuleByIdAsPortal()
        {
            var portalRules = await _portalRepo.GetPortalRules();
            var portalRule = await _portalRepoAsPortal.GetPortalRule(portalRules.FirstOrDefault().Id);
            Assert.IsNotNull(portalRule);
            Assert.IsInstanceOfType(portalRule, typeof(PortalRuleDetailDto));
        }

        [TestMethod]
        public async Task Portal_GetPortalRulesForSpecificPortalAsUser()
        {
            var portals = await _portalRepo.GetPortals();
            var portalRules = await _portalRepo.GetPortalRules(portals.FirstOrDefault().Id);
            Assert.IsNotNull(portalRules);
            Assert.IsInstanceOfType(portalRules, typeof(IEnumerable<PortalRuleDetailDto>));
            Assert.IsTrue(portalRules.Count > 0);
        }

        [TestMethod]
        public async Task Portal_GetPortalRulesForSpecificPortalAsPortal()
        {
            var portals = await _portalRepo.GetPortals();
            var portalRules = await _portalRepoAsPortal.GetPortalRules(portals.FirstOrDefault(t => t.SerialNumber == StringConstants.PortalUsername).Id);
            Assert.IsNotNull(portalRules);
            Assert.IsInstanceOfType(portalRules, typeof(IEnumerable<PortalRuleDetailDto>));
            Assert.IsTrue(portalRules.Count > 0);
        }

        [TestMethod]
        public async Task Portal_CreatePortalRule()
        {
            var addPortal = await CreateRandomPortalRule();
            var newPortalRule = await _portalRepo.CreatePortalRule(addPortal);
            Assert.IsNotNull(newPortalRule);
            Assert.IsInstanceOfType(newPortalRule, typeof(PortalRuleDetailDto));
            await _portalRepo.DeletePortalRule(newPortalRule.Id);
        }

        private async Task<AddPortalRuleDto> CreateRandomPortalRule()
        {
            var placeInId = (await _placeRepo.GetAllPlaces()).FirstOrDefault().Id;
            return new AddPortalRuleDto
            {
                Antennas = new List<Guid>(),
                Name = "Cat Portal Rule",
                PlaceInId = placeInId,
                PlaceOutId = null,
                RuleType = PortalRuleType.AntennaEvents,
                SensorInId = null,
                SensorOutId = null,
                Timeout = 3000
            };
        }

        [TestMethod]
        public async Task Portal_UpdatePortalRule()
        {
            var newPortalRule = await _portalRepo.CreatePortalRule(await CreateRandomPortalRule());
            var placeInId = (await _placeRepo.GetAllPlaces()).FirstOrDefault().Id;
            var update = new UpdatePortalRuleDto
            {
                Antennas = new List<Guid>(),
                Name = "Cat Portal Rule",
                PlaceInId = placeInId,
                PlaceOutId = null,
                RuleType = PortalRuleType.AntennaEvents,
                SensorInId = null,
                SensorOutId = null,
                Timeout = 5000
            };
            var updatedPortalRule = await _portalRepo.UpdatePortalRule(update);
            Assert.IsNotNull(updatedPortalRule);
            Assert.IsInstanceOfType(updatedPortalRule, typeof (PortalRuleDetailDto));
            Assert.IsTrue(updatedPortalRule.Timeout == update.Timeout);
            await _portalRepo.DeletePortalRule(updatedPortalRule.Id);
        }

        [TestMethod]
        public async Task Portal_DeletePortalRule()
        {
            var newPortalRule = await _portalRepo.CreatePortalRule(await CreateRandomPortalRule());
            await _portalRepo.DeletePortalRule(newPortalRule.Id);
            var deletedPortalRule = _portalRepo.GetPortalRule(newPortalRule.Id);
            Assert.IsNull(deletedPortalRule);            
        }
    }
}
