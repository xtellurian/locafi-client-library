using System;
using Locafi.Client.Data;

namespace Locafi.Client.Model.Query.Simple
{
    public class SimpleUserQuery : IRestQuery<UserDto>
    {
        public enum SearchParameter
        {
            UserName,
            Email,
            Surname,
            GivenName
        }

        public SimpleUserQuery(string searchTerm, SearchParameter searchParameter)
        {
            var result = $"{QueryStrings.FilterStart}{QueryStrings.Contains(GetPropertyName(searchParameter),searchTerm)}";
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
