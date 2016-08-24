using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Places;
using Locafi.Client.Model.Dto.Tags;
using Locafi.Client.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.UnitTests.EntityGenerators
{
    public static class PlaceGenerator
    {
        public static async Task<AddPlaceDto> GenerateRandomAddPlaceDto()
        {
            ITemplateRepo _templateRepo = WebRepoContainer.TemplateRepo;

            var ran = new Random(DateTime.UtcNow.Millisecond);
            var templates = await _templateRepo.GetTemplatesForType(TemplateFor.Place);
            var template = await _templateRepo.GetById(templates.Items.ElementAt(ran.Next(templates.Items.Count())).Id);
            var name = "Random - " + template.Name + " " + ran.Next().ToString();
            var description = name + " - Description";

            var addPlace = new AddPlaceDto(template)
            {
                Description = description,
                Name = name,
                PlaceTagList = new List<WriteTagDto>()
                {
                    new WriteTagDto()
                    {
                        TagNumber = Guid.NewGuid().ToString(),
                        TagType = TagType.PassiveRfid
                    }
                }
            };

            return addPlace;
        }
    }
}
