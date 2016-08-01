using Locafi.Client.Authentication;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Repo;
using Locafi.Client.UnitTests;
using Locafi.Script.Factory;
using Locafi.Script.Implementations;

namespace Locafi.Script
{
    public static class WebRepoContainer
    {
        private static readonly ISerialiserService Serialiser;
        private static IAuthorisedHttpTransferConfigService AuthorisedHttpTransferConfigService 
            => HttpConfigFactory.Generate(StringConstants.BaseUrl, StringConstants.EmailAddress, StringConstants.Password).Result;
        private static readonly IHttpTransferConfigService HttpConfigService;


        public static IItemRepo ItemRepo => new ItemRepo(AuthorisedHttpTransferConfigService, Serialiser);
        public static IInventoryRepo InventoryRepo => new InventoryRepo(AuthorisedHttpTransferConfigService, Serialiser);
        public static IOrderRepo OrderRepo => new OrderRepo(AuthorisedHttpTransferConfigService,Serialiser);
        public static IPlaceRepo PlaceRepo => new PlaceRepo(AuthorisedHttpTransferConfigService, Serialiser);
        public static IPersonRepo PersonRepo => new PersonRepo(AuthorisedHttpTransferConfigService, Serialiser);
        public static IDeviceRepo DeviceRepo => new DeviceRepo(AuthorisedHttpTransferConfigService,Serialiser);
        public static IReasonRepo ReasonRepo => new ReasonRepo(AuthorisedHttpTransferConfigService,Serialiser);
        public static ISnapshotRepo SnapshotRepo => new SnapshotRepo(AuthorisedHttpTransferConfigService, Serialiser);
        public static ISkuRepo SkuRepo => new SkuRepo(AuthorisedHttpTransferConfigService, Serialiser);
        public static ITagReservationRepo TagReservationRepo => new TagReservationRepo(AuthorisedHttpTransferConfigService, Serialiser);
        public static ITemplateRepo TemplateRepo => new TemplateRepo(AuthorisedHttpTransferConfigService,Serialiser);
        public static IUserRepo UserRepo => new UserRepo(AuthorisedHttpTransferConfigService, Serialiser);
        public static IAuthenticationRepo AuthRepo => new AuthenticationRepo(HttpConfigService, Serialiser);
        public static ICycleCountRepo CycleCountRepo => new CycleCountRepo(AuthorisedHttpTransferConfigService, Serialiser);



        static WebRepoContainer()
        {
            Serialiser = new Serialiser();
            //AuthorisedHttpTransferConfigService = HttpConfigFactory.Generate(StringConstants.BaseUrl, UserName, Password).Result;
            HttpConfigService = new UnauthorisedHttpTransferConfigService();
        }
    }
}
