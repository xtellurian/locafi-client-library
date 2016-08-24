using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Templates;
using Locafi.Client.Model.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Locafi.Client.Model;
using Locafi.Client.UnitTests.EntityGenerators;
using Locafi.Client.Model.Dto.ExtendedProperties;
using System.Linq;
using Locafi.Client.UnitTests.Validators;
using Locafi.Client.Model.Query.Builder;
using Locafi.Client.Model.Query;
using Locafi.Client.UnitTests.Extensions;

namespace Locafi.Client.UnitTests.Tests
{
    [TestClass]
    public class TemplateRepoTests
    {
        private ITemplateRepo _templateRepo;
        private IExtendedPropertyRepo _extPropRepo;
        private List<Guid> _extPropsToCleanup;
        private IList<Guid> _templatesToCleanup;

        [TestInitialize]
        public void Initialise()
        {
            _templateRepo = WebRepoContainer.TemplateRepo;
            _extPropRepo = WebRepoContainer.ExtendedPropertyRepo;
            _extPropsToCleanup = new List<Guid>();
            _templatesToCleanup = new List<Guid>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            // delete all that were created
            foreach (var Id in _templatesToCleanup)
            {
                 _templateRepo.DeleteTemplate(Id).Wait();
            }

            // delete all that were created
            foreach (var Id in _extPropsToCleanup)
            {
                _extPropRepo.DeleteExtendedProperty(Id).Wait();
            }
        }

        [TestMethod]
        public async Task Template_CreateBasic()
        {
            // create
            var addDto = await TemplateGenerator.GenerateRandomAddTemplateDto();
            var detail = await _templateRepo.CreateTemplate(addDto);
            _templatesToCleanup.AddUnique(detail.Id);
            _extPropsToCleanup.AddRangeUnique(detail.TemplateExtendedPropertyList.Select(p => p.ExtendedPropertyId));

            // check result
            TemplateDtoValidator.TemplateDetailCheck(detail);
            Validator.IsTrue(string.Equals(addDto.Name, detail.Name));
            Validator.IsTrue(string.Equals(addDto.TemplateType, detail.TemplateType));
            Validator.IsTrue(detail.TemplateExtendedPropertyList.Count == addDto.TemplateExtendedPropertyList.Count);
        }

        [TestMethod]
        public async Task Template_CreateWithExtProps()
        {
            // create
            var addDto = await TemplateGenerator.GenerateRandomAddTemplateDto(null,2);
            var detail = await _templateRepo.CreateTemplate(addDto);
            _templatesToCleanup.AddUnique(detail.Id);
            _extPropsToCleanup.AddRangeUnique(detail.TemplateExtendedPropertyList.Select(p => p.ExtendedPropertyId));

            // check result
            TemplateDtoValidator.TemplateDetailCheck(detail);
            Validator.IsTrue(string.Equals(addDto.Name, detail.Name));
            Validator.IsTrue(string.Equals(addDto.TemplateType, detail.TemplateType));
            Validator.IsTrue(detail.TemplateExtendedPropertyList.Count == addDto.TemplateExtendedPropertyList.Count);
        }

        [TestMethod]
        public async Task Template_GetAll()
        {
            // create
            var addDto = await TemplateGenerator.GenerateRandomAddTemplateDto();
            var detail = await _templateRepo.CreateTemplate(addDto);
            _templatesToCleanup.AddUnique(detail.Id);
            _extPropsToCleanup.AddRangeUnique(detail.TemplateExtendedPropertyList.Select(p => p.ExtendedPropertyId));

            // check result
            TemplateDtoValidator.TemplateDetailCheck(detail);
            Validator.IsTrue(string.Equals(addDto.Name, detail.Name));
            Validator.IsTrue(string.Equals(addDto.TemplateType, detail.TemplateType));
            Validator.IsTrue(detail.TemplateExtendedPropertyList.Count == addDto.TemplateExtendedPropertyList.Count);

            // get all
            var templates = await _templateRepo.QueryTemplates();

            // check result
            Validator.IsNotNull(templates);
            Validator.IsInstanceOfType(templates,typeof(PageResult<TemplateSummaryDto>));
        }

        [TestMethod]
        public async Task Template_GetById()
        {
            // create
            var addDto = await TemplateGenerator.GenerateRandomAddTemplateDto();
            var detail = await _templateRepo.CreateTemplate(addDto);
            _templatesToCleanup.AddUnique(detail.Id);
            _extPropsToCleanup.AddRangeUnique(detail.TemplateExtendedPropertyList.Select(p => p.ExtendedPropertyId));

            // check result
            TemplateDtoValidator.TemplateDetailCheck(detail);
            Validator.IsTrue(string.Equals(addDto.Name, detail.Name));
            Validator.IsTrue(string.Equals(addDto.TemplateType, detail.TemplateType));
            Validator.IsTrue(detail.TemplateExtendedPropertyList.Count == addDto.TemplateExtendedPropertyList.Count);

            // get the template
            var template = await _templateRepo.GetById(detail.Id);

            // check result
            TemplateDtoValidator.TemplateDetailCheck(template);
            Validator.IsTrue(string.Equals(addDto.Name, template.Name));
            Validator.IsTrue(string.Equals(addDto.TemplateType, template.TemplateType));
            Validator.IsTrue(template.TemplateExtendedPropertyList.Count == addDto.TemplateExtendedPropertyList.Count);
        }

        [TestMethod]
        public async Task Template_Update()
        {
            // create
            var addDto = await TemplateGenerator.GenerateRandomAddTemplateDto(TemplateFor.Item,2);
            var detail = await _templateRepo.CreateTemplate(addDto);
            _templatesToCleanup.AddUnique(detail.Id);
            _extPropsToCleanup.AddRangeUnique(detail.TemplateExtendedPropertyList.Select(p => p.ExtendedPropertyId));

            // check result
            TemplateDtoValidator.TemplateDetailCheck(detail);
            Validator.IsTrue(string.Equals(addDto.Name, detail.Name));
            Validator.IsTrue(string.Equals(addDto.TemplateType, detail.TemplateType));
            Validator.IsTrue(detail.TemplateExtendedPropertyList.Count == addDto.TemplateExtendedPropertyList.Count);

            // get the template
            var template = await _templateRepo.GetById(detail.Id);

            // check result
            TemplateDtoValidator.TemplateDetailCheck(template);
            Validator.IsTrue(string.Equals(addDto.Name, template.Name));
            Validator.IsTrue(string.Equals(addDto.TemplateType, template.TemplateType));
            Validator.IsTrue(template.TemplateExtendedPropertyList.Count == addDto.TemplateExtendedPropertyList.Count);

            // update the template
            var newPropDto = ExtendedPropertyGenerator.GenerateRandomAddExtPropDto(template.TemplateType);
            var newProp = await _extPropRepo.CreateExtendedProperty(newPropDto);
            _extPropsToCleanup.AddUnique(newProp.Id);
            var updateDto = new UpdateTemplateDto(template)
            {
                Name = template.Name + " - Update",
                TemplateExtendedPropertyList = new List<AddTemplateExtendedPropertyDto>() { new AddTemplateExtendedPropertyDto() { ExtendedPropertyId = newProp.Id } }
            };
            var updateDetail = await _templateRepo.UpdateTemplate(updateDto);

            // check result
            TemplateDtoValidator.TemplateDetailCheck(updateDetail);
            Validator.IsTrue(string.Equals(updateDto.Name, updateDetail.Name));
            Validator.IsTrue(string.Equals(addDto.TemplateType, updateDetail.TemplateType));
            Validator.IsTrue(template.TemplateExtendedPropertyList.Count != updateDetail.TemplateExtendedPropertyList.Count);
            Validator.IsTrue(updateDto.TemplateExtendedPropertyList.Count == updateDetail.TemplateExtendedPropertyList.Count);
        }

        [TestMethod]
        public async Task Template_GetForType()
        {
            foreach(var e in Enum.GetNames(typeof(TemplateFor)))
            {
                Array TemplateForValues = Enum.GetValues(typeof(TemplateFor));

                // create a template for each type
                foreach (var type in TemplateForValues)
                {
                    // create
                    var addDto = await TemplateGenerator.GenerateRandomAddTemplateDto((TemplateFor)type, 2);
                    var detail = await _templateRepo.CreateTemplate(addDto);
                    _templatesToCleanup.AddUnique(detail.Id);
                    _extPropsToCleanup.AddRangeUnique(detail.TemplateExtendedPropertyList.Select(p => p.ExtendedPropertyId));
                }

                // now try and get a template for each type, there should be at least one
                foreach (var type in TemplateForValues)
                {
                    var templates = await _templateRepo.GetTemplatesForType((TemplateFor)type);
                    Validator.IsNotNull(templates, "Template query: null");
                    Validator.IsInstanceOfType(templates, typeof(PageResult<TemplateSummaryDto>));
                    Validator.IsTrue(templates.Items.Count() > 0, "Template query: no templates");
                    foreach (var t in templates)
                    {
                        Validator.AreEqual(t.TemplateType, (TemplateFor)type, "Template query: wrong template type");
                    }
                }
            }
        }

        [TestMethod]
        public async Task Template_Delete()
        {
            // create
            var addDto = await TemplateGenerator.GenerateRandomAddTemplateDto(null,2);
            var detail = await _templateRepo.CreateTemplate(addDto);
            _templatesToCleanup.AddUnique(detail.Id);
            _extPropsToCleanup.AddRangeUnique(detail.TemplateExtendedPropertyList.Select(p => p.ExtendedPropertyId));

            // check result
            TemplateDtoValidator.TemplateDetailCheck(detail);
            Validator.IsTrue(string.Equals(addDto.Name, detail.Name));
            Validator.IsTrue(string.Equals(addDto.TemplateType, detail.TemplateType));
            Validator.IsTrue(detail.TemplateExtendedPropertyList.Count == addDto.TemplateExtendedPropertyList.Count);

            // get the template
            var deleteReult = await _templateRepo.DeleteTemplate(detail.Id);

            // check the result
            Validator.IsTrue(deleteReult);
            // remove from delete list
            _templatesToCleanup.Remove(detail.Id);

            // verify
            var query = QueryBuilder<TemplateSummaryDto>.NewQuery(t => t.Id, detail.Id, ComparisonOperator.Equals).Build();
            var queryResult = await _templateRepo.QueryTemplates(query);
            Validator.IsFalse(queryResult.Items.Contains(detail));

            // verify with get
            try
            {
                var sameItem = await _templateRepo.GetById(detail.Id);

                Validator.IsTrue(false, "Deleted entity returned");
            }
            catch (Exception e)
            {
                // this is expected                
            }
        }
    }
}
