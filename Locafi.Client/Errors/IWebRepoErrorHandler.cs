using System.Net.Http;
using System.Threading.Tasks;

namespace Locafi.Client.Errors
{
    public interface IWebRepoErrorHandler
    {
        Task Handle (HttpResponseMessage responseMessage);
    }
}
