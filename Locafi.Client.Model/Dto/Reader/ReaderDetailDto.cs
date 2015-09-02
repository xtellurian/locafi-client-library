using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Reader
{
    public class ReaderDetailDto : ReaderSummaryDto
    {
        public string Description { get; set; }

        public List<AntennaConfigDto> AntennaConfigs { get; set; }

        public string LoginName { get; set; }

        public string Password { get; set; }

        public string ReaderMode { get; set; }  // ReaderMode Enum //TODO: make enum
    }
}
