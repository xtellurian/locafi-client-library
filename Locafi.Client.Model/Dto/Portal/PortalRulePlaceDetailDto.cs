using System;

namespace Locafi.Client.Model.Dto.Portal
{
    public class PortalRulePlaceDetailDto
    {
        public Guid Id { get; set; }

        public long PlaceInId { get; set; }

        public string PlaceInName { get; set; }

        public Guid? SensorInId { get; set; }

        public string SensorInName { get; set; }

        public long? PlaceOutId { get; set; }

        public string PlaceOutName { get; set; }

        public Guid? SensorOutId { get; set; }

        public string SensorOutName { get; set; }
    }
}
