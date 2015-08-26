﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Data;

namespace Locafi.Client.Model.Query
{
    public class SimplePersonQuery : ISimpleRestQuery<PersonDto>
    {
        public enum StringParameters
        {
            TagNumber,
            GivenName,
            Surname,
            EmailAddress
        }
        public SimplePersonQuery(string searchTerm, StringParameters parameter)
        {
            var result = $"{QueryStrings.FilterStart}{QueryStrings.Contains(searchTerm, GetPropertyName(parameter))}";
            _queryString = result;
        }

        private string GetPropertyName(StringParameters parameter)
        {
            switch (parameter)
            {
                case StringParameters.EmailAddress:
                    return "EmailAddress";
                case StringParameters.GivenName:
                    return "GivenName";
                case StringParameters.Surname:
                    return "Surname";
                case StringParameters.TagNumber:
                    return "TagNumber";
            }
            throw new ArgumentException("Unknown string property");
        }

        private string _queryString;
        public string AsRestQuery()
        {
            return _queryString;
        }
    }
}