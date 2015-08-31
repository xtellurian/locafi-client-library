using System;

namespace Locafi.Client.Model.Dto.Skus
{
    public class SkuSummaryDto : EntityDtoBase
    {
        public string Name { get; set; }

        public Guid TemplateId { get; set; }
        
        public string TemplateName { get; set; }
    }
}
