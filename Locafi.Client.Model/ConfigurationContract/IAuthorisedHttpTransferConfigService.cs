namespace Locafi.Client.Model.ConfigurationContract
{
    public interface IAuthorisedHttpTransferConfigService : IHttpTransferConfigService
    { 
        string GetTokenString();
    }
}