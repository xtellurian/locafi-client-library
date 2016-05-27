using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Skus
{
    public class UpdateSkuDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string CompanyPrefix { get; set; }

        public string ItemReference { get; set; }

        public string CustomPrefix { get; set; }

        public Guid ItemTemplateId { get; set; }

        public string SkuNumber { get; set; }

        public IList<WriteSkuExtendedPropertyDto> SkuExtendedPropertyList { get; set; }

        public UpdateSkuDto()
        {
            SkuExtendedPropertyList = new List<WriteSkuExtendedPropertyDto>();
        }
    }
}
