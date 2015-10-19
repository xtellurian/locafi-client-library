using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Contract.Http
{
    public class SimpleHttpTransferer : IHttpTransferer
    {
        public async Task<HttpResponseMessage> GetResponse(HttpMethod method, string url, string content = null, string authToken = null)
        {
            var message = new HttpRequestMessage(method, url);
            if (content != null) message.Content = new StringContent(content, Encoding.UTF8, "application/json");

            //message.Content.Headers.Add("Content-Type", new List<string> { "application/json" });
            if (authToken != null)
            {
                message.Headers.Add("Authorization", "Token " + authToken);
            }

            var client = new HttpClient();
            Debug.WriteLine($"{method} request at {url}");
            if (content != null) Debug.WriteLine($"Payload:\n {content}");
            var response = await client.SendAsync(message);
            var serverMessage = await response.Content.ReadAsStringAsync();
            Debug.WriteLine(response.IsSuccessStatusCode
                ? $"{method} request success at {url}"
                : $"Error executing {method} request at {url}");
            Debug.WriteLine($"Response from server:\n{serverMessage}");
            return response;
        }
    }
}
