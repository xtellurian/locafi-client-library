using System;
using Locafi.Client.Model.Dto.Items;

namespace Locafi.Client.Model.RelativeUri
{
    public static class ItemUri
    {
        public static string ServiceName => "Items";
        public static string GetItems => "GetFilteredItems";
        public static string CreateItem => @"CreateItem";
        public static string UploadItems => @"UploadItems";
        public static string UpdatePlace => @"UpdatePlace";
        public static string UpdateTag => @"UpdateTag";
        public static string UpdateItem => @"UpdateItem";
        public static string SearchItems => @"SearchItems";
        public static string ClearItems => @"ClearSelectedItemState";
        public static string ConsumeItems => @"ConsumeItems";
        public static string GetItemPlaceHistory => @"GetItemPlaceHistory";
        public static string GetItemStateHistory => @"GetItemStateHistory";

        public static string GetItem(Guid id)
        {
            return $"GetItem/{id}";
        }

        public static string DeleteItem(Guid id)
        {
            return $"DeleteItem/{id}";
        }
    }
}
