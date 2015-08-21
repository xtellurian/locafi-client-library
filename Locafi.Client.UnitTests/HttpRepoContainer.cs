using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Services.Contract;
using Locafi.Client.Services.Repo;
using Locafi.Client.UnitTests.Factory;
using Locafi.Client.UnitTests.Implementations;

namespace Locafi.Client.UnitTests
{
    public abstract class HttpRepoContainer
    {
        protected IItemRepo ItemRepo;
        protected IOrderRepo OrderRepo;
        protected IPlaceRepo PlaceRepo;
        protected IPersonRepo PersonRepo;
        protected ISnapshotRepo SnapshotRepo;
        protected IUserRepo UserRepo;

        protected HttpRepoContainer(string baseUrl, string userName, string password)
        {
            var serialiser = new Serialiser();
            var httpTransferConfigService = HttpConfigFactory.Generate(baseUrl, userName, password).Result;
            OrderRepo = new OrderRepo(httpTransferConfigService, serialiser);
            PlaceRepo = new PlaceRepo(httpTransferConfigService, serialiser);
            PersonRepo = new PersonRepo(httpTransferConfigService, serialiser);
            SnapshotRepo = new SnapshotRepo(httpTransferConfigService,serialiser);
            ItemRepo = new ItemRepo(httpTransferConfigService,serialiser);
        }
    }
}
