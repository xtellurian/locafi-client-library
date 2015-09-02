using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Reader
{
    public class ReaderSummaryDto : EntityDtoBase
    {
        public string Name { get; set; }

        public string IPAddress { get; set; }

        public string ReaderType { get; set; }  // ReaderType Enum

        public string SerialNumber { get; set; }
    }
}
