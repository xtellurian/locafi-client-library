using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Orders
{
    public class AddOrderSkuDto
    {
        public Guid SkuId { get; set; }

        public int RequiredCount { get; set; }

    }
}
