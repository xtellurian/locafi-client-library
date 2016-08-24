using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Reasons;
using Locafi.Client.Model.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Locafi.Client.Model.Query.PropertyComparison;
using Locafi.Client.Model.Query;
using System.Linq;
using Locafi.Client.UnitTests.Validators;
using Locafi.Client.Model;
using Locafi.Client.UnitTests.Extensions;

namespace Locafi.Client.UnitTests.Tests
{
    [TestClass]
    public class ReasonRepoTests
    {
        private IReasonRepo _reasonRepo;
        private IList<Guid> _reasonsToDelete;

        [TestInitialize]
        public void Initialise()
        {
            _reasonRepo = WebRepoContainer.ReasonRepo;
            _reasonsToDelete = new List<Guid>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            // delete all reasons that were created
            foreach (var reasonId in _reasonsToDelete)
            {
                _reasonRepo.Delete(reasonId).Wait();
            }
        }

        [TestMethod]
        public async Task Reason_Create()
        {
            // create a reason
            var reason = CreateRandomAddReasonDto();
            var result = await _reasonRepo.CreateReason(reason);
            _reasonsToDelete.AddUnique(result.Id);

            // check the result
            ReasonDtoValidator.ReasonDetailCheck(result);
            Validator.IsTrue(string.Equals(reason.Description, result.Description));
            Validator.IsTrue(string.Equals(reason.ReasonNumber, result.ReasonNumber));
        }

        [TestMethod]
        public async Task Reason_Get()
        {
            // create a reason to get
            var addDto = CreateRandomAddReasonDto();
            var result = await _reasonRepo.CreateReason(addDto);
            _reasonsToDelete.AddUnique(result.Id);

            // check the result
            ReasonDtoValidator.ReasonDetailCheck(result);

            // query reasons
            var reason = await _reasonRepo.GetReason(result.Id);

            // check the result
            ReasonDtoValidator.ReasonDetailCheck(reason);
            Validator.AreEqual(result.ReasonNumber, reason.ReasonNumber);
            Validator.AreEqual(result.Description, reason.Description);
        }

        [TestMethod]
        public async Task Reason_GetAll()
        {
            // create a reason so there is at least 1 to return
            var reason = CreateRandomAddReasonDto();
            var result = await _reasonRepo.CreateReason(reason);
            _reasonsToDelete.AddUnique(result.Id);

            // check the result
            ReasonDtoValidator.ReasonDetailCheck(result);

            // query reasons
            var reasons = await _reasonRepo.QueryReasons();
            Validator.IsNotNull(reasons);
            Validator.IsInstanceOfType(reasons, typeof(PageResult<ReasonSummaryDto>));
            Validator.IsTrue(reasons.Items.Count() > 0);
        }

        [TestMethod]
        public async Task Reason_Update()
        {
            // create a reason
            var addDto = CreateRandomAddReasonDto();
            var result = await _reasonRepo.CreateReason(addDto);
            _reasonsToDelete.AddUnique(result.Id);

            // check the result
            ReasonDtoValidator.ReasonDetailCheck(result);

            // Update the reason
            var updateDto = new UpdateReasonDto(result)
            {
                ReasonNumber = result.ReasonNumber + " - Update",
                Description = result.Description + " - Update"
            };
            var updateDetail = await _reasonRepo.UpdateReason(updateDto);

            // check the result
            ReasonDtoValidator.ReasonDetailCheck(updateDetail);
            Validator.AreEqual(updateDetail.Id, result.Id);
            Validator.AreNotEqual(updateDetail.ReasonNumber, result.ReasonNumber);
            Validator.AreNotEqual(updateDetail.Description, result.Description);
            Validator.AreEqual(updateDetail.ReasonNumber, updateDto.ReasonNumber);
            Validator.AreEqual(updateDetail.Description, updateDto.Description);
        }

        [TestMethod]
        public async Task Reason_Delete()
        {
            // create a reason so there is at least 1 to return
            var reason = CreateRandomAddReasonDto();
            var result = await _reasonRepo.CreateReason(reason);
            _reasonsToDelete.AddUnique(result.Id);

            // check the result
            ReasonDtoValidator.ReasonDetailCheck(result);

            // delete the reason
            var deleteResult = await _reasonRepo.Delete(result.Id);

            // check the result
            Validator.IsTrue(deleteResult);
            // remove from delete list
            _reasonsToDelete.Remove(result.Id);

            // verify
            var allReasons = await _reasonRepo.QueryReasons();
            Validator.IsFalse(allReasons.Items.Contains(result));   

            // verify with get
            try
            {
                var sameItem = await _reasonRepo.GetReason(result.Id);

                Validator.IsTrue(false, "Deleted entity returned");
            }
            catch (Exception e)
            {
                // this is expected                
            }
        }

        #region PrivateMethods
        private AddReasonDto CreateRandomAddReasonDto()
        {
            var ran = new Random(DateTime.UtcNow.Millisecond);
            var name = Guid.NewGuid().ToString();
            var number = Guid.NewGuid().ToString();
            var reason = new AddReasonDto
            {
                Description = name,
                ReasonNumber = number
            };
            return reason;
        }
        #endregion
    }
}
