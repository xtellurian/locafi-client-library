using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Contract.Config
{
    public interface IHttpTransferConfig
    {
        string BaseUrl { get; }

        /// <summary>
        /// Add any headers, including Auth if required
        /// </summary>
        Dictionary<string, string> Headers { get; }

    }
}
