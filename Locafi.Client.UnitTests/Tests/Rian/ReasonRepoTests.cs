using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Reasons;
using Locafi.Client.Model.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests
{
    [TestClass]
    public class ReasonRepoTests
    {
        private IReasonRepo _reasonRepo;
        private IList<Guid> _toBeDeleted;

        [TestInitialize]
        public void Initialise()
        {
            _reasonRepo = WebRepoContainer.ReasonRepo;
            _toBeDeleted = new List<Guid>();
        }
  //      [TestMethod]
        public async Task Reason_GetAll()
        {
            var reasons = await _reasonRepo.GetAllReasons();
            Assert.IsNotNull(reasons);
            Assert.IsInstanceOfType(reasons,typeof(IEnumerable<ReasonDetailDto>));
        }

        [TestMethod]
        public async Task Reason_Create()
        {
            var reason = MakeRandomAddReason();
            var result = await _reasonRepo.CreateReason(reason);
            Assert.IsNotNull(result);
            _toBeDeleted.Add(result.Id);
            Assert.IsInstanceOfType(result,typeof(ReasonDetailDto));
            Assert.IsTrue(string.Equals(reason.Name, result.Name));
            Assert.IsTrue(string.Equals(reason.ReasonNo, result.ReasonNo));
            
        }

      //  [TestMethod]
        public async Task Reason_Delete()
        {
            var reason = MakeRandomAddReason();
            var result = await _reasonRepo.CreateReason(reason);
            Assert.IsNotNull(result);
            await _reasonRepo.Delete(result.Id);
            var allReasons = await _reasonRepo.GetAllReasons();
            Assert.IsFalse(allReasons.Contains(result));
        }

        [TestCleanup]
        public async void Cleanup()
        {
            foreach (var g in _toBeDeleted)
            {
                await _reasonRepo.Delete(g);
            }
        }


        private AddReasonDto MakeRandomAddReason()
        {
            var ran = new Random();
            var name = Guid.NewGuid().ToString();
            var number = Guid.NewGuid().ToString();
            var values = Enum.GetValues(typeof(ReasonFor));
            var reasonFor = (ReasonFor)values.GetValue(ran.Next(values.Length));
            var reason = new AddReasonDto
            {
                Name = name,
                ReasonNo = number,
                ReasonFor = reasonFor
            };
            return reason;
        }
    }
}
