using System;
using System.Collections.Generic;
using Locafi.Client.Model.Dto.Tags;

namespace Locafi.Client.Model.Dto.OutboundIntegrations
{
    public class PersonIntegrationDto
    {
        public Guid Id { get; set; }

        public string GivenName { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public IList<TagDetailDto> TagList { get; set; }

        public IList<IntegrationExtendedProperty> ExtendedPropertyList { get; set; }

        public PersonIntegrationDto()
        {
            TagList = new List<TagDetailDto>();
            ExtendedPropertyList = new List<IntegrationExtendedProperty>();
        }
    }
}
