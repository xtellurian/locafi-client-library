using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Http;
using Locafi.Client.Model.RelativeUri;

namespace Locafi.Client.UnitTests.Mocks
{
    internal class MockHttpTransferrer : IHttpTransferer
    {
        private readonly bool _justReturnOk;

        public MockHttpTransferrer(bool justReturnOk = false)
        {
            _justReturnOk = justReturnOk;
        }

        private readonly IDictionary<string, IList<string>> _calls = new Dictionary<string, IList<string>>();
        public IDictionary<string, IList<string>> HttpCalls =>  _calls; 
        public async Task<HttpResponseMessage> GetResponse(HttpMethod method, string url, string content = null, string authToken = null, IDictionary<string, string> headers = null)
        {
            AddToCalls(url, content);
            if (url.Contains(ErrorLogUri.ServiceName))
            {

                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(content)
                };
            }
            else if(_justReturnOk)
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            throw new NotImplementedException("This is a mock class");
        }

        private void AddToCalls(string url, string content)
        {
            if (_calls.ContainsKey(url))
            {
                _calls[url].Add(content); // add the content
            }
            else
            {
                _calls.Add(new KeyValuePair<string, IList<string>>(url, new List<string> {content})); // add the key and add content
                
            }
        }
    }
}
