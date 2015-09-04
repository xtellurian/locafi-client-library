using System;

namespace Locafi.Client.Model.Dto.Places
{
    public class PlaceSummaryDto : EntityDtoBase
    {

        public string Name { get; set; }

        public Guid TemplateId { get; set; }
        public string TemplateName { get; set; }

        public string TagNumber { get; set; }
        public string TagTypeName { get; set; }
    }
}
