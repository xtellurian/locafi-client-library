using Locafi.Client.Authentication;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Crypto;
using Locafi.Client.Repo;
using Locafi.Client.UnitTests.Factory;
using Locafi.Client.UnitTests.Implementations;

namespace Locafi.Client.UnitTests
{
    public static class WebRepoAsAuthorisedReaderContainer
    {
        private static readonly ISerialiserService Serialiser;
        private static readonly IHttpTransferConfigService HttpConfigService;
        private static IAuthorisedHttpTransferConfigService AuthorisedHttpTransferConfigService
            => HttpConfigFactory.Generate(StringConstants.BaseUrl, StringConstants.ReaderUserName, GetReaderPassword(StringConstants.ReaderUserName), true).Result;

        private static string GetReaderPassword(string readerUserName)
        {
            var hasher = new Sha256HashService();
            return hasher.GenerateHash(StringConstants.Secret, readerUserName);
        }

        public static IAuthenticationRepo AuthRepo => new AuthenticationRepo(HttpConfigService, Serialiser);
        public static IDeviceRepo DeviceRepo => new DeviceRepo(AuthorisedHttpTransferConfigService, Serialiser);

        static WebRepoAsAuthorisedReaderContainer()
        {
            Serialiser = new Serialiser();            
            HttpConfigService = new UnauthorisedHttpTransferConfigService();            
        }

        
    }
}
