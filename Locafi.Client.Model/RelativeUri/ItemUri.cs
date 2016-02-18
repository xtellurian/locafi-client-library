using System;
using Locafi.Client.Model.Dto.Items;

namespace Locafi.Client.Model.RelativeUri
{
    public static class ItemUri
    {
        public static string ServiceName => "Items";
        public static string GetItemCount => @"GetItems/GetCount";
        public static string GetItems => "GetItems";
        public static string CreateItem => @"CreateItem";

        public static string GetItem(Guid id)
        {
            return $"GetItem/{id}";
        } 

        /// <summary>
        /// Creates relative URI to update this Item's Place - ie - moves an Item
        /// </summary>
        /// <param name="updateItemPlaceDto">The DTO defined to update an Item's place</param>
        /// <returns>The relative URI to be appended ie BASE_URL + SERVICE + THIS </returns>
        public static string UpdatePlace(UpdateItemPlaceDto updateItemPlaceDto)
        {
            return $"{GetItem(updateItemPlaceDto.ItemId)}/UpdatePlace";
        }

        /// <summary>
        /// Creates relative URI to update this Item's Tag
        /// </summary>
        /// <param name="updateItemTagDto">The DTO defined to update an Item's tag</param>
        /// <returns>The relative URI to be appended ie BASE_URL + SERVICE + THIS</returns>
        public static string UpdateTag(UpdateItemTagDto updateItemTagDto)
        {
            return $"{GetItem(updateItemTagDto.ItemId)}/UpdateTag";
        }

        /// <summary>
        /// Creates relative URI to update this Item
        /// </summary>
        /// <param name="updateItemDto">The DTO defined to update an Item</param>
        /// <returns>The relative URI to be appended ie BASE_URL + SERVICE + THIS</returns>
        public static string UpdateUri(UpdateItemDto updateItemDto)
        {
            return $"{GetItem(updateItemDto.ItemId)}/UpdateItem";
        }

        public static string SearchItemUri()
        {
            return $"SearchItems";
        }

        public static string DeleteItem(Guid id)
        {
            return $"DeleteItem/{id}";
        }
    }
}
