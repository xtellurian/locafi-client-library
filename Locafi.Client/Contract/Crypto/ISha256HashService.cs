namespace Locafi.Client.Contract.Crypto
{
    public interface ISha256HashService
    {
        string GenerateHash(string secret, string data);
    }
}