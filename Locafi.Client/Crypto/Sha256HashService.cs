using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Crypto;
using PCLCrypto;

namespace Locafi.Client.Crypto
{
    public class Sha256HashService : ISha256HashService
    {
        public string GenerateHash(string data)
        {
           var bytes = GetBytes(data);
            var hasher = WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(HashAlgorithm.Sha1);
            var hash = hasher.HashData(bytes);
            var hashBase64 = Convert.ToBase64String(hash);
            return hashBase64;
        }

        private static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}
