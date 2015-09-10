// This is a contract file
// Breaking changes to this contract should be accompanied by a version change in the third version number
// ie ... 1.2.42.11 -> 1.2.43.0

using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Locafi.Client.Model.Responses;

namespace Locafi.Client.Contract.Errors
{
    public interface IWebRepoErrorHandler
    {
        Task Handle (HttpResponseMessage responseMessage);
        Task Handle(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode);
    }
}
