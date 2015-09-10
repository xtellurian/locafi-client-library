using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Templates;
using Locafi.Client.Model.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests
{
    [TestClass]
    public class TemplateRepoTests
    {
        private ITemplateRepo _templateRepo;

        [TestInitialize]
        public void Initialise()
        {
            _templateRepo = WebRepoContainer.TemplateRepo;
        }
        [TestMethod]
        public async Task Template_Create()
        {
            
        }

        [TestMethod]
        public async Task Template_GetAll()
        {
            var templates = await _templateRepo.GetAllTemplates();
            Assert.IsNotNull(templates);
            Assert.IsInstanceOfType(templates,typeof(IEnumerable<TemplateSummaryDto>));
        }

        [TestMethod]
        public async Task Template_GetById()
        {
            var templates = await _templateRepo.GetAllTemplates();
            foreach (var template in templates)
            {
                var detail = await _templateRepo.GetById(template.Id);
                Assert.IsNotNull(detail);
                Assert.IsInstanceOfType(detail, typeof(TemplateDetailDto));
                Assert.IsTrue(detail.Id == template.Id);
            }
        }
        [TestMethod]
        public async Task Template_GetForEveryType()
        {
            foreach(var e in Enum.GetNames(typeof(TemplateFor)))
            {
                TemplateFor target;
                Enum.TryParse(e, out target);
                var templates = await _templateRepo.GetTemplatesForType(target);
                Assert.IsNotNull(templates);
                Assert.IsInstanceOfType(templates, typeof(IList<TemplateSummaryDto>));
                foreach (var t in templates)
                {
                    Assert.AreEqual(t.TemplateType, target);
                }
            }
        }


    }
}
