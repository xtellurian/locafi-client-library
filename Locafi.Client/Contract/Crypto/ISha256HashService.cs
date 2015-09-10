// This is a contract file
// Breaking changes to this contract should be accompanied by a version change in the third version number
// ie ... 1.2.42.11 -> 1.2.43.0

namespace Locafi.Client.Contract.Crypto
{
    public interface ISha256HashService
    {
        string GenerateHash(string secret, string data);
    }
}