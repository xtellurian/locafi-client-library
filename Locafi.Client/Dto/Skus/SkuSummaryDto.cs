using System;

namespace Locafi.Client.Model.Dto.Skus
{
    public class SkuSummaryDto
    {

        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid TemplateId { get; set; }
        
        public string TemplateName { get; set; }

        public Guid? CreatedByUserId { get; set; }

        public string CreatedByUserFullName { get; set; }

        public DateTime DateCreated { get; set; }

        public Guid? LastModifiedByUserId { get; set; }

        public string LastModifiedByUserFullName { get; set; }

        public DateTime? DateLastModified { get; set; }
    }
}
