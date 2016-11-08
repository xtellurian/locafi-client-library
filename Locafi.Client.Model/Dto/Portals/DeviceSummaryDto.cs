using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Portals
{
    public class DeviceSummaryDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string DeviceType { get; set; }

        public string ConnectionType { get; set; }

        public string SerialNumber { get; set; }

        public Guid TemplateId { get; set; }

        public string TemplateName { get; set; }

    }
}
