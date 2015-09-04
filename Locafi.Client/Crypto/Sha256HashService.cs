using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Crypto;
using Org.BouncyCastle.Crypto.Digests;

namespace Locafi.Client.Crypto
{
    public class Sha256HashService : ISha256HashService
    {
        public string GenerateHash(string secret, string data)
        {
            var key = GetBytes(secret);
            var dataBytes = GetBytes(data);
            var hasher = new HMACSHA256(key);
            var result = hasher.ComputeHash(dataBytes);
            var hashBase64 = Convert.ToBase64String(result);
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
