namespace Locafi.Client.Contract.Config
{
    public interface IAuthorisedHttpTransferConfigService : IHttpTransferConfigService
    { 
        string GetTokenString();
    }
}