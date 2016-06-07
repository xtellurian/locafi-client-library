using Locafi.Client.Model.Dto.Tags;
using System;
using System.Collections.Generic;

namespace Locafi.Client.Model.Dto.Items
{
    public class UpdateItemTagDto
    {
        public Guid Id { get; set; }

        public IList<WriteTagDto> ItemTagList { get; set; }

        public UpdateItemTagDto()
        {
            ItemTagList = new List<WriteTagDto>();
        }

        public static UpdateItemTagDto FromItemDetail(ItemDetailDto detail)
        {
            return new UpdateItemTagDto
            {
                Id = detail.Id
            };
        }
    }
}