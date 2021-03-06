﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Items;
using Locafi.Client.Model.Dto.Orders;

namespace Locafi.Client.Contract.Processors
{
    public interface IProcessTagResult
    {
        bool IsRecognised { get; }
        ReadOrderSkuDto SkuLineItem { get; }
        ReadOrderItemDto ItemLineItem { get; }
        string Gtin { get; }

    }
}
