using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Http;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Dto.ErrorLogs;
using Locafi.Client.Model.Enums;
using Locafi.Client.Model.RelativeUri;
using Locafi.Client.Model.Responses;

namespace Locafi.Client.Repo
{
    public class ErrorRepo : WebRepo, IErrorRepo
    {
        public ErrorRepo(IHttpTransferer transferer, IAuthorisedHttpTransferConfigService authorisedUnauthorizedConfigService, ISerialiserService serialiser) 
            : base(transferer, authorisedUnauthorizedConfigService, serialiser, ErrorLogUri.ServiceName)
        {
        }

        public async Task<ErrorLogDetailDto> LogException (Exception ex, ErrorLevel level = ErrorLevel.Minor)
        {
            var dto = CreateDtoFromException(ex, level);
            return await this.LogArbitrary(dto);
        }

        public async Task<ErrorLogDetailDto> LogArbitrary(AddErrorLogDto error)
        {
            var path = ErrorLogUri.CreateErrorlog;
            return await base.Post<ErrorLogDetailDto>(error, path);

        }

        private AddErrorLogDto CreateDtoFromException(Exception exception, ErrorLevel level)
        {
            var dto = new AddErrorLogDto()
            {
                ErrorLevel = level,
                TimeStamp = DateTime.Now,
                ErrorMessage = exception.Message,
                ErrorDetails = $"StackTrace : \n  {exception.StackTrace} \n  **END STACKTRACE**  \n  InnerException: \n {exception.InnerException?.Message ?? "null"} \n **END INNER EXCEPTION"
            };
            return dto;
        }


        public override async Task Handle(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode)
        {
            throw new ErrorRepoException(serverMessages, statusCode);
        }

        public override async Task Handle(HttpResponseMessage response)
        {
            throw new ErrorRepoException(await response.Content.ReadAsStringAsync());
        }
    }
}
