using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Locafi.Client.Model.Responses;

namespace Locafi.Client.Contract.Errors
{
    public interface IWebRepoErrorHandler
    {
        Task Handle (HttpResponseMessage responseMessage);
        Task Handle(IEnumerable<CustomResponseMessage> serverMessages);
    }
}
