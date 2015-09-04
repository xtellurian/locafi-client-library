using System.Collections.Generic;

namespace Locafi.Client.Model.Dto.Places
{
    public class PlaceDetailDto : PlaceSummaryDto
    {
        public string Description { get; set; }
        public int TagType { get; set; }

        public long UsageCount { get; set; }

        public IList<ReadEntityExtendedPropertyDto> PlaceExtendedPropertyList { get; set; }

    }
}
