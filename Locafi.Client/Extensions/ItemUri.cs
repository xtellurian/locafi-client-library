using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Items;

namespace Locafi.Client.Model.Extensions
{
    public static class ItemUri
    {
        public static string UpdatePlaceUri(this UpdateItemPlaceDto updateItemPlaceDto)
        {
            return $"GetItem/{updateItemPlaceDto.ItemId}/UpdatePlace";
        }

        public static string UpdateTagUri(this UpdateItemTagDto updateItemTagDto)
        {
            return $"GetItem/{updateItemTagDto.ItemId}/UpdatePlace";
        }

        public static string UpdateUri(this UpdateItemDto updateItemDto)
        {
            return $"GetItem/{updateItemDto.ItemId}/UpdateItem";
        }
    }
}
