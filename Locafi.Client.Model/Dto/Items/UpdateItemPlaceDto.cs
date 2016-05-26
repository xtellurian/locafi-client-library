using System;

namespace Locafi.Client.Model.Dto.Items
{
    public class UpdateItemPlaceDto
    {
        public Guid Id { get; set; }

        public Guid NewPlaceId { get; set; }

        public Guid? TagId { get; set; } 

        public static UpdateItemPlaceDto FromItemDetail(ItemSummaryDto summary)
        {
            return new UpdateItemPlaceDto
            {
                Id = summary.Id
            };
        }
    }
}