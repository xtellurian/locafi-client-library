using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Devices;
using Locafi.Client.Repo;
using Locafi.Client.UnitTests.Implementations;
using Locafi.Client.UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests.Rian.Cache
{
    [TestClass]
    public class CachedClusterRepoTest
    {
        [TestMethod]
        public async Task CacheUnitTests_InsertIntoCache()
        {
            var mockCache = new MockClusterCache();
            var serialiser = new Serialiser();
            var tran = new MockHttpTransferrer();
            var config = new MockAuthorisedHttpConfigService();

            var repo = new ClusterCachedRepo(tran, config, serialiser, mockCache);
            
            var id = Guid.NewGuid().ToString();
            var placeId = Guid.NewGuid();

            var result =
                await
                    repo.ProcessCluster(new ClusterDto
                    {
                        Id = id,
                        PlaceId = placeId,
                        Tags = new List<ClusterTagDto>(),
                        TimeStamp = DateTime.Now
                    });


            Assert.IsTrue(mockCache.Cache.Any(c => c.Id == id));
        }
    }
}
