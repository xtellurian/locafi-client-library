﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.PortalDtos
{
    public class ReadAntennaDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int AntennaNumber { get; set; }

        public bool IsEnabled { get; set; }

        public double TxPower { get; set; }
    }
}
