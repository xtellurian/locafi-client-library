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
using Locafi.Client.Contract.Repo;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Dto.InboundIntegrations;
using Locafi.Client.Model.RelativeUri;
using Locafi.Client.Model.Responses;

namespace Locafi.Client.Repo
{
    public class FileImportRepo : WebRepo, IWebRepoErrorHandler, IFileImportRepo
    {
        public FileImportRepo(IAuthorisedHttpTransferConfigService authorisedConfigService, ISerialiserService serialiser) : base(new SimpleHttpTransferer(), authorisedConfigService, serialiser, FileImportUri.ServiceName)
        {
        }

        public FileImportRepo(IHttpTransferer transferer, IAuthorisedHttpTransferConfigService configService, ISerialiserService serialiser) : base(transferer, configService, serialiser, FileImportUri.ServiceName)
        {
        }

        public async Task<FileUploadResultDto> ImportItems(FileUploadDto uploadDto)
        {
            var path = FileImportUri.ImportItems;
            var result = await Post<FileUploadResultDto>(uploadDto, path);
            return result;
        }

        public async Task<FileUploadResultDto> ImportPlaces(FileUploadDto uploadDto)
        {
            var path = FileImportUri.ImportPlaces;
            var result = await Post<FileUploadResultDto>(uploadDto, path);
            return result;
        }

        public async Task<FileUploadResultDto> ImportPersons(FileUploadDto uploadDto)
        {
            var path = FileImportUri.ImportPersons;
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
