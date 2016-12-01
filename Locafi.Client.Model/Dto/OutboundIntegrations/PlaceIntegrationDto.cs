using System;
using System.Collections.Generic;
using Locafi.Client.Model.Dto.Tags;

namespace Locafi.Client.Model.Dto.OutboundIntegrations
{
    public class PlaceIntegrationDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IList<TagDetailDto> TagList { get; set; }

        public IList<IntegrationExtendedProperty> ExtendedPropertyList { get; set; }

        public PlaceIntegrationDto()
        {
            TagList = new List<TagDetailDto>();
            ExtendedPropertyList = new List<IntegrationExtendedProperty>();
        }
    }
}
