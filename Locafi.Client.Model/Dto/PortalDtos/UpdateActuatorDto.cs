﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.PortalDtos
{
    public class UpdateActuatorDto : AddActuatorDto
    {
        public Guid? Id { get; set; }
    }
}
