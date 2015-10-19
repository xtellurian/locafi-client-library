﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Devices
{
    public class AddIpConfigDto
    {
        public string IpAddress { get; set; }

        public string SubnetMask { get; set; }

        public int? TcpPort { get; set; }

        public int? UdpPort { get; set; }

        public string MacAddress { get; set; }

        public string Hostname { get; set; }
    }
}