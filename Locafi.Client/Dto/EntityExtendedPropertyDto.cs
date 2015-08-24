using System;

namespace LegacyNavigatorApi.Models.Dtos
{

    // ReSharper disable once ClassNeverInstantiated.Global
    public class WriteEntityExtendedPropertyDto
    {
        public Guid TemplateExtendedPropertyId { get; set; }

        public Guid ExtendedPropertyId { get; set; }

        public string Value { get; set; }
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    public class ReadEntityExtendedPropertyDto
    {
        public Guid Id { get; set; }

        public Guid ExtendedPropertyId { get; set; }

        public Guid TemplateId { get; set; }

        public string TemplateType { get; set; }

        public string ExtendedPropertyName { get; set; }

        public string ExtendedPropertyDescription { get; set; }

        public string TemplateIsRequired { get; set; }

        public string ExtendedPropertyDataType { get; set; }

        public string Value { get; set; }

    }
}
