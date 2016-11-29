using Locafi.Client.Authentication;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Crypto;
using Locafi.Client.Repo;
using Locafi.Client.UnitTests.Factory;
using Locafi.Client.UnitTests.Implementations;

namespace Locafi.Client.UnitTests
{
    public static class WebRepoAgentContainer
    {
        private static readonly ISerialiserService Serialiser;
        private static readonly IHttpTransferConfigService HttpConfigService;
        private static IAuthorisedHttpTransferConfigService AuthorisedHttpTransferConfigService
            => HttpConfigFactory.GenerateAgent(StringConstants.BaseUrl, StringConstants.HardwareKey).Result;

        public static IAuthenticationRepo AuthRepo => new AuthenticationRepo(HttpConfigService, Serialiser);

        public static IPortalRepo PortalRepo => new PortalRepo(AuthorisedHttpTransferConfigService, Serialiser);

        static WebRepoAgentContainer()
        {
            Serialiser = new Serialiser();            
            HttpConfigService = new UnauthorisedHttpTransferConfigService();            
        }

        
    }
}
