﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Query
{
    public interface ISimpleRestQuery<T>
    {
        string AsRestQuery();
    }
}
