using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Persons;
using Locafi.Client.Model.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Locafi.Client.Model.Dto.Tags;
using Locafi.Client.Exceptions;
using Locafi.Client.UnitTests.Validators;
using Locafi.Client.Model;
using Locafi.Client.Model.Query.Builder;
using Locafi.Client.Model.Dto.Places;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Dto.ExtendedProperties;
using Locafi.Client.UnitTests.EntityGenerators;
using Locafi.Client.UnitTests.Extensions;

namespace Locafi.Client.UnitTests.Tests
{
    [TestClass]
    public class ExpentedPropertyRepoTests
    {
        private IExtendedPropertyRepo _extPropRepo;
        private ITemplateRepo _templateRepo;
        private IList<Guid> _extPropsToCleanup;

        [TestInitialize]
        public void Initialise()
        {
            _extPropRepo = WebRepoContainer.ExtendedPropertyRepo;
            _templateRepo = WebRepoContainer.TemplateRepo;
            _extPropsToCleanup = new List<Guid>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            // delete all that were created
            foreach (var Id in _extPropsToCleanup)
            {
                _extPropRepo.DeleteExtendedProperty(Id).Wait();
            }
        }

        [TestMethod]
        public async Task ExtProp_Create()
        {
            // create
            var addDto = ExtendedPropertyGenerator.GenerateRandomAddExtPropDto();
            var detail = await _extPropRepo.CreateExtendedProperty(addDto);
            _extPropsToCleanup.AddUnique(detail.Id);

            // check result
            ExtendedPropertyDtoValidator.ExtendedPropertyDetailCheck(detail);
            Validator.IsTrue(string.Equals(addDto.Name,detail.Name));
            Validator.IsTrue(string.Equals(addDto.Description, detail.Description));
            Validator.IsTrue(string.Equals(addDto.DataType, detail.DataType));
            Validator.IsTrue(string.Equals(addDto.TemplateType, detail.TemplateType));
        }

        [TestMethod]
        public async Task ExtProp_Get()
        {
            // create
            var addDto = ExtendedPropertyGenerator.GenerateRandomAddExtPropDto();
            var detail = await _extPropRepo.CreateExtendedProperty(addDto);
            _extPropsToCleanup.AddUnique(detail.Id);

            // check result
            ExtendedPropertyDtoValidator.ExtendedPropertyDetailCheck(detail);
            Validator.IsTrue(string.Equals(addDto.Name, detail.Name));
            Validator.IsTrue(string.Equals(addDto.Description, detail.Description));
            Validator.IsTrue(string.Equals(addDto.DataType, detail.DataType));
            Validator.IsTrue(string.Equals(addDto.TemplateType, detail.TemplateType));

            // get the detail
            detail = await _extPropRepo.GetExtendedPropertyById(detail.Id);

            // check result
            ExtendedPropertyDtoValidator.ExtendedPropertyDetailCheck(detail);
            Validator.IsTrue(string.Equals(addDto.Name, detail.Name));
            Validator.IsTrue(string.Equals(addDto.Description, detail.Description));
            Validator.IsTrue(string.Equals(addDto.DataType, detail.DataType));
            Validator.IsTrue(string.Equals(addDto.TemplateType, detail.TemplateType));
        }

        [TestMethod]
        public async Task ExtProp_GetAll()
        {
            // create
            var addDto = ExtendedPropertyGenerator.GenerateRandomAddExtPropDto();
            var detail = await _extPropRepo.CreateExtendedProperty(addDto);
            _extPropsToCleanup.AddUnique(detail.Id);

            // query
            var extProps = await _extPropRepo.QueryExtendedProperties();

            // check result
            Validator.IsNotNull(extProps, "extProps != null");
            Validator.IsInstanceOfType(extProps, typeof(PageResult<ExtendedPropertySummaryDto>));
            Validator.IsTrue(extProps.Items.Count() > 0);
            Validator.IsTrue(extProps.Contains(detail));
        }

        [TestMethod]
        public async Task ExtProp_Update()
        {
            // create
            var addDto = ExtendedPropertyGenerator.GenerateRandomAddExtPropDto();
            var detail = await _extPropRepo.CreateExtendedProperty(addDto);
            _extPropsToCleanup.AddUnique(detail.Id);

            // check result
            ExtendedPropertyDtoValidator.ExtendedPropertyDetailCheck(detail);
            Validator.IsTrue(string.Equals(addDto.Name, detail.Name));
            Validator.IsTrue(string.Equals(addDto.Description, detail.Description));
            Validator.IsTrue(string.Equals(addDto.DataType, detail.DataType));
            Validator.IsTrue(string.Equals(addDto.TemplateType, detail.TemplateType));

            // get the detail
            detail = await _extPropRepo.GetExtendedPropertyById(detail.Id);

            // check result
            ExtendedPropertyDtoValidator.ExtendedPropertyDetailCheck(detail);
            Validator.IsTrue(string.Equals(addDto.Name, detail.Name));
            Validator.IsTrue(string.Equals(addDto.Description, detail.Description));
            Validator.IsTrue(string.Equals(addDto.DataType, detail.DataType));
            Validator.IsTrue(string.Equals(addDto.TemplateType, detail.TemplateType));

            // update the ext prop
            var updateDto = new UpdateExtendedPropertyDto(detail)
            {
                Name = detail.Name + " - Update",
                Description = detail.Description + " - Update"
            };
            var updateDetail = await _extPropRepo.UpdateExtendedProperty(updateDto);

            // check result
            ExtendedPropertyDtoValidator.ExtendedPropertyDetailCheck(updateDetail);
            Validator.IsFalse(string.Equals(detail.Name, updateDetail.Name));
            Validator.IsFalse(string.Equals(detail.Description, updateDetail.Description));
            Validator.IsTrue(string.Equals(updateDto.Name, updateDetail.Name));
            Validator.IsTrue(string.Equals(updateDto.Description, updateDetail.Description));
            Validator.IsTrue(string.Equals(addDto.DataType, updateDetail.DataType));
            Validator.IsTrue(string.Equals(addDto.TemplateType, updateDetail.TemplateType));
        }

        [TestMethod]
        public async Task ExtProp_Delete()
        {
            // create person
            var addDto = ExtendedPropertyGenerator.GenerateRandomAddExtPropDto();
            var detail = await _extPropRepo.CreateExtendedProperty(addDto);
            _extPropsToCleanup.AddUnique(detail.Id);

            // check result
            ExtendedPropertyDtoValidator.ExtendedPropertyDetailCheck(detail);
            Validator.IsTrue(string.Equals(addDto.Name, detail.Name));
            Validator.IsTrue(string.Equals(addDto.Description, detail.Description));
            Validator.IsTrue(string.Equals(addDto.DataType, detail.DataType));
            Validator.IsTrue(string.Equals(addDto.TemplateType, detail.TemplateType));

            // get the detail
            detail = await _extPropRepo.GetExtendedPropertyById(detail.Id);

            // check result
            ExtendedPropertyDtoValidator.ExtendedPropertyDetailCheck(detail);
            Validator.IsTrue(string.Equals(addDto.Name, detail.Name));
            Validator.IsTrue(string.Equals(addDto.Description, detail.Description));
            Validator.IsTrue(string.Equals(addDto.DataType, detail.DataType));
            Validator.IsTrue(string.Equals(addDto.TemplateType, detail.TemplateType));

            // delete the entity
            var deleteResult = await _extPropRepo.DeleteExtendedProperty(detail.Id);
            Validator.IsTrue(deleteResult);

            // verify
            var query = QueryBuilder<ExtendedPropertySummaryDto>.NewQuery(p => p.Id, detail.Id, ComparisonOperator.Equals).Build();
            var queryResult = await _extPropRepo.QueryExtendedProperties(query); // get the entity again
            Validator.IsFalse(queryResult.Any(p => p.Id == detail.Id)); // check our entity is actually gone

            // remove from delete list
            _extPropsToCleanup.Remove(detail.Id);

            // verify with get
            try
            {
                detail = await _extPropRepo.GetExtendedPropertyById(detail.Id);

                Validator.IsTrue(false, "Deleted entity returned");
            }
            catch (Exception e)
            {
                // this is expected                
            }
        }
    }
}
