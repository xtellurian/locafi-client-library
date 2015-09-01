using System.Collections.Generic;

namespace Locafi.Client.Model.Dto.Skus
{
    public class SkuDetailDto : SkuSummaryDto
    {
//        public Guid Id { get; set; }

//        public string Name { get; set; }

        public string Description { get; set; }

        public string CompanyPrefix { get; set; }

        public string ItemReference { get; set; }

//        public Guid TemplateId { get; set; }

//        public string TemplateName { get; set; }

//        public Guid CreatedByUserId { get; set; }

//        public string CreatedByUserFullName { get; set; }

//        public DateTime DateCreated { get; set; }

//        public Guid LastModifiedByUserId { get; set; }

//        public Guid LastModifiedByUserFullName { get; set; }

//        public DateTime DateLastModified { get; set; }

        public IList<ReadSkuExtendedPropertyDto> SkuExtendedPropertyList { get; set; }

        public SkuDetailDto()
        {
            SkuExtendedPropertyList = new List<ReadSkuExtendedPropertyDto>();
        }
    }
}
