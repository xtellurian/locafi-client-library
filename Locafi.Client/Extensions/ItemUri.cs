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
        /// <summary>
        /// Creates relative URI to update this Item's Place - ie - moves an Item
        /// </summary>
        /// <param name="updateItemPlaceDto">The DTO defined to update an Item's place</param>
        /// <returns>The relative URI to be appended ie BASE_URL + SERVICE + THIS </returns>
        public static string UpdatePlaceUri(this UpdateItemPlaceDto updateItemPlaceDto)
        {
            return $"GetItem/{updateItemPlaceDto.ItemId}/UpdatePlace";
        }

        /// <summary>
        /// Creates relative URI to update this Item's Tag
        /// </summary>
        /// <param name="updateItemTagDto">The DTO defined to update an Item's tag</param>
        /// <returns>The relative URI to be appended ie BASE_URL + SERVICE + THIS</returns>
        public static string UpdateTagUri(this UpdateItemTagDto updateItemTagDto)
        {
            return $"GetItem/{updateItemTagDto.ItemId}/UpdatePlace";
        }

        /// <summary>
        /// Creates relative URI to update this Item
        /// </summary>
        /// <param name="updateItemDto">The DTO defined to update an Item</param>
        /// <returns>The relative URI to be appended ie BASE_URL + SERVICE + THIS</returns>
        public static string UpdateUri(this UpdateItemDto updateItemDto)
        {
            return $"GetItem/{updateItemDto.ItemId}/UpdateItem";
        }
    }
}
