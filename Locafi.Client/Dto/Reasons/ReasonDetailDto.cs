using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Reasons
{
    public class ReasonDetailDto : DtoBase
    {
        public Guid Id { get; set; }

        public string ReasonNo { get; set; }

        public string Name { get; set; }

        public string ReasonFor { get; set; }

        public override bool Equals(object obj)
        {
            var reason = obj as ReasonDetailDto;
            return reason != null && reason.Id == this.Id;
        }
    }
}
