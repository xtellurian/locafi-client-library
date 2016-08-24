using Locafi.Client.Model.Dto.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Persons
{
    public class UpdatePersonDto
    {
        public Guid Id { get; set; }

        public string GivenName { get; set; }

        public string Surname { get; set; }

        public Guid TemplateId { get; set; }

        public string Email { get; set; }

        public string ImageUrl { get; set; }

        public IList<WriteEntityExtendedPropertyDto> PersonExtendedPropertyList { get; set; }

        public UpdatePersonDto()
        {
            PersonExtendedPropertyList = new List<WriteEntityExtendedPropertyDto>();
        }

        public UpdatePersonDto(PersonDetailDto detail)
        {
            Id = detail.Id;
            GivenName = detail.GivenName;
            Surname = detail.Surname;
            TemplateId = detail.TemplateId;
            Email = detail.Email;
            ImageUrl = detail.ImageUrl;
            PersonExtendedPropertyList =
                new List<WriteEntityExtendedPropertyDto>(detail.PersonExtendedPropertyList);
        }

        public UpdatePersonDto(UserDetailDto detail)
        {
            Id = detail.PersonId;
            GivenName = detail.GivenName;
            Surname = detail.Surname;
            TemplateId = detail.TemplateId;
            Email = detail.Email;
            ImageUrl = detail.ImageUrl;
            PersonExtendedPropertyList =
                new List<WriteEntityExtendedPropertyDto>(detail.PersonExtendedPropertyList);
        }

        public static UpdatePersonDto FromPersonDetail(PersonDetailDto detail)
        {
            return new UpdatePersonDto
            {
                Email = detail.Email,
                GivenName = detail.GivenName,
                PersonExtendedPropertyList =
                    new List<WriteEntityExtendedPropertyDto>(detail.PersonExtendedPropertyList),
                Id = detail.Id,
                Surname = detail.Surname,
                TemplateId = detail.TemplateId,
                ImageUrl = detail.ImageUrl
            };
        }
    }
}
