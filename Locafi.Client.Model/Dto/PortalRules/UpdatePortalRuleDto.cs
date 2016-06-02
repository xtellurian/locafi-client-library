﻿using System;
using System.Collections.Generic;
using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Locafi.Client.Model.Dto.PortalRules
{
    public class UpdatePortalRuleDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public PortalRuleType RuleType { get; set; }

        public int Timeout { get; set; }

        public IList<Guid> Antennas { get; set; }

        public Guid PlaceInId { get; set; }

        public Guid? SensorInId { get; set; }

        public Guid? PlaceOutId { get; set; }

        public Guid? SensorOutId { get; set; }
    }
}