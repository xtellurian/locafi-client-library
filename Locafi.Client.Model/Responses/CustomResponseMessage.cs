using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Responses
{
    public class CustomResponseMessage
    {
        public ResponseCodes ErrorCode { get; set; }

        public string Message { get; set; }

        public string Field { get; set; }

    }
}
