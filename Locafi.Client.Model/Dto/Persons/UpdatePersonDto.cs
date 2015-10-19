﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Persons
{
    public class UpdatePersonDto
    {
        public Guid PersonId { get; set; }

        public string GivenName { get; set; }

        public string Surname { get; set; }

        public string EmailAddress { get; set; }

        public IList<WriteEntityExtendedPropertyDto> PersonExtendedPropertyList { get; set; }

        public UpdatePersonDto()
        {
            PersonExtendedPropertyList = new List<WriteEntityExtendedPropertyDto>();
        }

        public static UpdatePersonDto FromPersonDetail(PersonDetailDto detail)
        {
            return new UpdatePersonDto
            {
                EmailAddress = detail.EmailAddress,
                GivenName = detail.GivenName,
                PersonExtendedPropertyList =
                    new List<WriteEntityExtendedPropertyDto>(detail.PersonExtendedPropeertyList),
                PersonId = detail.Id,
                Surname = detail.Surname

            };
        }
    }
}
