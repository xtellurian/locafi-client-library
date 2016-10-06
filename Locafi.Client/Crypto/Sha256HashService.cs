using System;
using System.Diagnostics;
using Locafi.Client.Contract.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;

namespace Locafi.Client.Crypto
{
    public class Sha256HashService : ISha256HashService
    {
        public string GenerateHash(string secret, string data)
        {
            var hmac = new HMac(new Sha256Digest());
            hmac.Init(new KeyParameter(GetBytes(secret)));
            var result = new byte[hmac.GetMacSize()];
            var bytes = GetBytes(data);

            hmac.BlockUpdate(bytes, 0, bytes.Length);
            hmac.DoFinal(result, 0);
            var hashBase64 = Convert.ToBase64String(result);
            return hashBase64;
        }

        private static byte[] GetBytes(string str)
        {
            var bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public string GenerateBluSignature(string appId, string requestHttpMethod, string requestUri, string sharedKey)
        {
            string data = $"{appId}{requestHttpMethod}{requestUri.ToLower()}";

            var hmac = new HMac(new Sha256Digest());
            hmac.Init(new KeyParameter(GetBytes(sharedKey)));
            var result = new byte[hmac.GetMacSize()];
            var bytes = GetBytes(data);

            hmac.BlockUpdate(bytes, 0, bytes.Length);
            hmac.DoFinal(result, 0);
            var calculated = Convert.ToBase64String(result);

#if DEBUG
            Debug.WriteLine($"Calculated: {calculated}");
#endif
            return calculated;
        }
    }
}
