﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Items;

namespace Locafi.Client.Model.Query
{
    public class SimpleItemQuery :ISimpleRestQuery<ItemSummaryDto>
    {
        public enum StringProperties
        {
            Name,
            Description
        }

        public enum IdProperties
        {
            SkuId
        }
        public SimpleItemQuery(string searchTerm, StringProperties stringProperties)
        {
            var result = $"{QueryStrings.FilterStart}{QueryStrings.Contains(searchTerm, GetPropertyName(stringProperties))}";
            _queryString = result;
        }

        public SimpleItemQuery(Guid id, IdProperties idProperties)
        {
            var result =
                $"{QueryStrings.FilterStart}{QueryStrings.Equals(GetPropertyName(idProperties), id.ToString())}";
            _queryString = result;
        }

        private string GetPropertyName(StringProperties stringProperty)
        {
            switch (stringProperty)
            {
                case StringProperties.Name:
                    return "Name";
                case StringProperties.Description: 
                    return "Description";
            }
            throw new ArgumentException("unknown string property");
        }

        private string GetPropertyName(IdProperties idProperties)
        {
            switch (idProperties)
            {
                case IdProperties.SkuId:
                    return "SkuId";
            }
            throw new ArgumentException("unknown Id property");
        }

        private readonly string _queryString;

        public string AsRestQuery()
        {
            return _queryString;
        }
    }
}