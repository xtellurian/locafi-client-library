﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Enums
{
    public enum FileUploadOperation
    {
        CreateOrUpdate,
        UpdateOnly,
        CreateIgnoreExisting,
        Delete
    }
}
