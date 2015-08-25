using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Services;
using Locafi.Client.Services.Authentication;
using Locafi.Client.Services.Repo;
using Locafi.Client.UnitTests.Factory;
using Locafi.Client.UnitTests.Implementations;

namespace Locafi.Client.UnitTests
{
    public static class WebRepoContainer
    {
        // config
        //private const string BaseUrl = @"http://legacynavapi.azurewebsites.net/api/";
        
        private const string UserName = "administrator";
        private const string Password = "ramp123";


        private static readonly ISerialiserService Serialiser;
        private static IAuthorisedHttpTransferConfigService AuthorisedHttpTransferConfigService => HttpConfigFactory.Generate(StringConstants.BaseUrl, UserName, Password).Result;
        private static readonly IHttpTransferConfigService HttpConfigService;


        public static IItemRepo ItemRepo => new ItemRepo(AuthorisedHttpTransferConfigService, Serialiser);
        public static IOrderRepo OrderRepo => new OrderRepo(AuthorisedHttpTransferConfigService,Serialiser);
        public static IPlaceRepo PlaceRepo => new PlaceRepo(AuthorisedHttpTransferConfigService, Serialiser);
        public static IPersonRepo PersonRepo => new PersonRepo(AuthorisedHttpTransferConfigService, Serialiser);
        public static ISnapshotRepo SnapshotRepo => new SnapshotRepo(AuthorisedHttpTransferConfigService, Serialiser);
        public static IUserRepo UserRepo => new UserRepo(AuthorisedHttpTransferConfigService, Serialiser);
        public static IAuthenticationRepo AuthRepo => new AuthenticationRepo(HttpConfigService, Serialiser);

        static WebRepoContainer()
        {
            Serialiser = new Serialiser();
            //AuthorisedHttpTransferConfigService = HttpConfigFactory.Generate(StringConstants.BaseUrl, UserName, Password).Result;
            HttpConfigService = new UnauthorisedHttpTransferConfigService();
        }
    }
}
