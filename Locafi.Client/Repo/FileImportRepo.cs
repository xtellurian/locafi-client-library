using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.ErrorHandlers;
using Locafi.Client.Contract.Http;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Dto.InboundIntegrations;
using Locafi.Client.Model.RelativeUri;
using Locafi.Client.Model.Responses;

namespace Locafi.Client.Repo
{
    public class FileImportRepo : WebRepo, IWebRepoErrorHandler
    {
        public FileImportRepo(IHttpTransferer transferer, IAuthorisedHttpTransferConfigService authorisedConfigService, ISerialiserService serialiser, string service) : base(transferer, authorisedConfigService, serialiser, service)
        {
        }

        public FileImportRepo(IHttpTransferer transferer, IHttpTransferConfigService configService, ISerialiserService serialiser, string service) : base(transferer, configService, serialiser, service)
        {
        }

        public async Task<FileUploadResultDto> ImportItems(FileUploadDto uploadDto)
        {
            var path = FileImportUri.ImportItems;
            var result = await Post<FileUploadResultDto>(uploadDto, path);
            return result;
        }

        public override Task Handle(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode, string url, string payload)
        {
            throw new FileRepoException(serverMessages, statusCode, url, payload);
        }

        public override async Task Handle(HttpResponseMessage response, string url, string payload)
        {
            throw new FileRepoException($"{url} -- {payload} -- " + await response.Content.ReadAsStringAsync());
        }
    }
}
