using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Services;
using Locafi.Client.Services.Repo;
using Locafi.Client.UnitTests.Factory;
using Locafi.Client.UnitTests.Implementations;

namespace Locafi.Client.UnitTests
{
    public static class WebRepoContainer
    {
        // config
        //private const string BaseUrl = @"http://legacynavapi.azurewebsites.net/api/";
        private const string BaseUrl = @"http://legacylocafiapiv2.azurewebsites.net/api/";
        private const string UserName = "administrator";
        private const string Password = "ramp123";


        private static readonly ISerialiserService Serialiser;
        private static readonly IHttpTransferConfigService HttpTransferConfigService;


        public static IItemRepo ItemRepo => new ItemRepo(HttpTransferConfigService, Serialiser);
        public static IOrderRepo OrderRepo => new OrderRepo(HttpTransferConfigService,Serialiser);
        public static IPlaceRepo PlaceRepo => new PlaceRepo(HttpTransferConfigService, Serialiser);
        public static IPersonRepo PersonRepo => new PersonRepo(HttpTransferConfigService, Serialiser);
        public static ISnapshotRepo SnapshotRepo => new SnapshotRepo(HttpTransferConfigService, Serialiser);
        public static IUserRepo UserRepo => new UserRepo(HttpTransferConfigService, Serialiser);


        static WebRepoContainer()
        {
            Serialiser = new Serialiser();
            HttpTransferConfigService = HttpConfigFactory.Generate(BaseUrl, UserName, Password).Result;
        }
    }
}
