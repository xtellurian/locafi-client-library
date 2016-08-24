using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Locafi.Client.Model.Dto.Reasons
{
    public class AddReasonDto
    {
        public string ReasonNumber { get; set; }

        public string Description { get; set; }
    }
}
