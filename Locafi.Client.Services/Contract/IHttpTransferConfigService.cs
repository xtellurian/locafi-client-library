using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Services.Contract
{
    public interface IHttpTransferConfigService
    {
        Task<string> GetBaseUrl();
        string GetTokenString();
       // void SetTokenString(string token);
    }
}