using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Errors;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Model.Dto.Reader;
using Locafi.Client.Model.Uri;

namespace Locafi.Client.Repo
{
    public class ReaderRepo : WebRepo, IWebRepoErrorHandler, IReaderRepo
    {
        public ReaderRepo(IAuthorisedHttpTransferConfigService authorisedUnauthorizedConfigService,
            ISerialiserService serialiser)
            : base(authorisedUnauthorizedConfigService, serialiser, ReaderUri.ServiceName)
        {
        }

        public async Task<IList<ReaderSummaryDto>> GetReaders()
        {
            var path = ReaderUri.GetReaders;
            var result = await Get<IList<ReaderSummaryDto>>(path);
            return result;
        }

        public async Task<ReaderDetailDto> GetReaderById(Guid id)
        {
            var path = ReaderUri.GetReader(id);
            var result = await Get<ReaderDetailDto>(path);
            return result;
        }

        public async Task DeleteReader(Guid id)
        {
            var path = ReaderUri.Delete(id);
            await Delete(path);
        }

        public async Task<ClusterReponseDto> ProcessCluster(ClusterDto cluster)
        {
            var path = ReaderUri.ProcessCluster;
            var result = await Post<ClusterReponseDto>(cluster,path);
            return result;
        }

        public async Task Handle(HttpResponseMessage responseMessage)
        {
            
        }
    }
}
