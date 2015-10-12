using System;

namespace Locafi.Client.Model.Dto.Items
{
    public class UpdateItemTagDto
    {
        public Guid ItemId { get; set; }

        public string NewTagId { get; set; }    // should this be tag number, also need to define tag type when updating tag


        public static UpdateItemTagDto FromItemDetail(ItemDetailDto detail)
        {
            return new UpdateItemTagDto
            {
                ItemId = detail.Id
            };
        }
    }
}