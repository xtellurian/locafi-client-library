using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Persons;
using Locafi.Client.Model.Dto.Tags;
using Locafi.Client.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.UnitTests.EntityGenerators
{
    public static class PersonGenerator
    {
        public static async Task<AddPersonDto> GenerateRandomAddPersonDto(Guid? placeId = null)
        {
            ITemplateRepo _templateRepo = WebRepoContainer.TemplateRepo;

            var ran = new Random(DateTime.UtcNow.Millisecond);
            var templates = await _templateRepo.GetTemplatesForType(TemplateFor.Person);
            var template = await _templateRepo.GetById(templates.Items.ElementAt(ran.Next(templates.Items.Count())).Id);
            var email = $"{Guid.NewGuid().ToString().Substring(0, 16)}@FakeDomain.com";
            var name = "Random - " + template.Name + " " + ran.Next().ToString();
            var surname = name + " - Surname";

            return new AddPersonDto(template)
            {
                TemplateId = template.Id,
                Email = email,
                GivenName = name,
                Surname = surname,
                ImageUrl = Guid.NewGuid().ToString(),
                PlaceId = placeId,
                PersonTagList = new List<WriteTagDto>()
                {
                    new WriteTagDto()
                    {
                        TagNumber = Guid.NewGuid().ToString(),
                        TagType = TagType.PassiveRfid
                    }
                }
            };
        }
    }
}
