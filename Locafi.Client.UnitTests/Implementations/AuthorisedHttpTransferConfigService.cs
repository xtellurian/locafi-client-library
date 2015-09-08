﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.ConfigurationContract;

namespace Locafi.Client.UnitTests.Implementations
{
    public class AuthorisedHttpTransferConfigService : IAuthorisedHttpTransferConfigService
    {
        public AuthorisedHttpTransferConfigService(string token)
        {
            _token = token;
        }

        public string BaseUrl { get; set; }

        private string _token;

        public string GetTokenString()
        {
            return _token;
        }

        public void SetTokenString(string token)
        {
            _token = token;
        }
    }
}
