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
using Locafi.Client.Contract.Repo.Cache;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Dto.Portals.Clusters;
using Locafi.Client.Model.RelativeUri;
using Locafi.Client.Model.Responses;
using Locafi.Client.Repo.Cache;

namespace Locafi.Client.Repo
{
    public class ClusterCachedRepo : CachedWebRepo, IClusterRepo
    {
        private readonly ICache<ClusterDto> _clusterCache;

        public ClusterCachedRepo(IHttpTransferer transferer, IAuthorisedHttpTransferConfigService authorisedConfigService, ISerialiserService serialiser, ICache<ClusterDto> clusterCache)
            : base(transferer, authorisedConfigService, serialiser, PortalUri.ServiceName)
        {
            _clusterCache = clusterCache;
        }

        public ClusterCachedRepo(IAuthorisedHttpTransferConfigService authorisedConfigService, ISerialiserService serialiser, ICache<ClusterDto> clusterCache)
            : base(new SimpleHttpTransferer(), authorisedConfigService, serialiser, PortalUri.ServiceName)
        {
            _clusterCache = clusterCache;
        }

        public async Task<bool> ProcessCluster(ClusterDto cluster)
        {
            var path = PortalUri.ProcessCluster;
            var result = await Post(cluster, path, _clusterCache);
            return result.Data;
        }

        public async Task FlushCache(int? amount = null)
        {
            await base.PostCache(_clusterCache, amount);
        }

        public override Task Handle(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode, string url, string payload)
        {
            throw new PortalRepoException(serverMessages, statusCode, url, payload);
        }

        public override async Task Handle(HttpResponseMessage response, string url, string payload)
        {
            throw new PortalRepoException($"{url} -- {payload} -- " + await response.Content.ReadAsStringAsync());
        }
    }
}
