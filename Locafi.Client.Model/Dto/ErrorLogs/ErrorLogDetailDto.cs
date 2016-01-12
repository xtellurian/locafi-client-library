﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.ErrorLogs
{
    public class ErrorLogDetailDto : ErrorLogSummaryDto
    {
        public string ErrorDetails { get; set; }
        public Guid? LoggedByUserId { get; set; }
    }
}
