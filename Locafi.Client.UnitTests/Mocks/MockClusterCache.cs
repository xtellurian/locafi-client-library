using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo.Cache;
using Locafi.Client.Model.Dto.Devices;

namespace Locafi.Client.UnitTests.Mocks
{
    internal class MockClusterCache : ICache<ClusterDto>
    {
        public List<ICachedEntity<ClusterDto>> Cache { get; set; }

        public MockClusterCache()
        {
            Cache = new List<ICachedEntity<ClusterDto>>();
        }
        public void Push(ICachedEntity<ClusterDto> entity)
        {
            Cache.Add(entity);
        }

        public IList<ICachedEntity<ClusterDto>> CopyCache(int? maxCopy = null)
        {
            return new List<ICachedEntity<ClusterDto>>(Cache);
        }

        public void Remove(string id)
        {
            Cache.RemoveAll(c => c.Id == id);
        }
    }
}
