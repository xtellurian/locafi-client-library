using System;

namespace Locafi.Client.Model.Dto.OutboundIntegrations
{
    public class IntegrationExtendedProperty
    {
        public Guid ExtendedPropertyId { get; set; }

        public string ExtendedPropertyName { get; set; }

        public string ExtendedPropertyDataType { get; set; }

        public string ExtendedPropertyValue { get; set; }
    }
}
