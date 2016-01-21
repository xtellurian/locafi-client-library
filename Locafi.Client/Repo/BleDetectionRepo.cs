using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Crypto;
using Locafi.Client.Contract.Http;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Crypto;
using Locafi.Client.Model.Dto.Ble;
using Newtonsoft.Json;

namespace Locafi.Client.Repo
{
    public class BleDetectionRepo : IBleDetectionRepo
    {
        private const string RelativeUrl = @"/api/Input";
        public BleDetectionRepo(IHttpTransferer transferer, ISha256HashService hashService, string baseUrl, string bluAppId, string privateKey)
        {
            _transferer = transferer;
            InitSignatureAndUrl(hashService, baseUrl, bluAppId, privateKey);
        }

        private void InitSignatureAndUrl(ISha256HashService hashService, string baseUrl, string bluAppId, string privateKey)
        {
            _uploadUrl = baseUrl.TrimEnd('/') + RelativeUrl;
            var sig = hashService.GenerateBluSignature(bluAppId, "POST", _uploadUrl, privateKey);
            _headerValue = $"blu {bluAppId};{sig}";
        }

        public BleDetectionRepo(string baseUrl, string bluAppId, string privateKey)
        {
            _transferer = new SimpleHttpTransferer();
            var hashService = new Sha256HashService();
            InitSignatureAndUrl(hashService, baseUrl, bluAppId, privateKey);
        }

        private readonly IHttpTransferer _transferer;
        private string _uploadUrl;
        private string _headerValue;

        public async Task UploadDetections(IEnumerable<BleDetectionBase> detections)
        {
            var dto = new BleDetectionCollection() {Detections = detections.ToList()};

            var serialisedData = JsonConvert.SerializeObject(dto);
            var response = await GetResponse(HttpMethod.Post, serialisedData); // get response

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException("Server returned 401");
            }

        }

        private async Task<HttpResponseMessage> GetResponse(HttpMethod method, string content = null)
        {
            var headers = new Dictionary<string, string> {{"Authorization", _headerValue}};
            var response = await _transferer.GetResponse(method, _uploadUrl, content, null, headers);
            return response;
        }

        private string GetFullPath(string baseUrl, string first, string second)
        {
            var result = new StringBuilder(baseUrl.TrimEnd('/'));
            var s1 = first.Trim('/');
            var isKeySelection = second.StartsWith("(");
            var isQuery = second.StartsWith("?");
            var s2 = second.Trim('/', '(', ')');
            if (isQuery) result.Append('/').Append(s1).Append(s2);
            else if (isKeySelection) result.Append('/').Append(s1).Append('(').Append(s2).Append(')');
            else result.Append('/').Append(s1).Append('/').Append(s2);
            return result.ToString();
        }
    }
}
