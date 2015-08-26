﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Data;

namespace Locafi.Client.Model.Query
{
    public class SimpleUserQuery : ISimpleRestQuery<UserDto>
    {
        public enum SearchParameter
        {
            UserName,
            Email,
            Surname,
            GivenName
        }

        public SimpleUserQuery(string searchTerm, SearchParameter searchSearchParameter)
        {
            GenerateQueryString(searchTerm, searchSearchParameter);
        }

        private void GenerateQueryString(string searchTerm, SearchParameter searchParameter)
        {
            var result = $"{QueryStrings.FilterStart}{QueryStrings.Contains(searchTerm,GetPropertyName(searchParameter))}";
            _queryString = result;
        }

        private string GetPropertyName(SearchParameter propertySearchParameter)
        {
            switch (propertySearchParameter)
            {
                case SearchParameter.Email:
                    return "EmailAddress";
                case SearchParameter.GivenName:
                    return "GivenName";
                case SearchParameter.Surname:
                    return "Surname";
                case SearchParameter.UserName:
                    return "UserName";
            }
            throw new ArgumentException("Unknown SearchParameter Enum");
        }

        private string _queryString;

        public string AsRestQuery()
        {
            return _queryString;
        }
    }
}