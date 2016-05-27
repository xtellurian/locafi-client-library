using System;

namespace Locafi.Client.Model.Dto.PortalRules
{
    public class PortalRulePlaceDetailDto
    {
        public Guid Id { get; set; }

        public Guid PlaceInId { get; set; }

        public string PlaceInName { get; set; }

        public Guid? SensorInId { get; set; }

        public string SensorInName { get; set; }

        public Guid? PlaceOutId { get; set; }

        public string PlaceOutName { get; set; }

        public Guid? SensorOutId { get; set; }

        public string SensorOutName { get; set; }
    }
}
