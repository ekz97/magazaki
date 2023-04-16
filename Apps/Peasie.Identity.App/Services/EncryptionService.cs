using System.Security.Cryptography;

namespace Peasie.Web.Services
{
    public class EncryptionService : IEncryptionService
    {
        public RSA GeneratePPKRandomly(out string privateKey, out string publicKey, int keySizeInBits = 2048)
        {
            var rsa = RSA.Create(keySizeInBits);
            privateKey = rsa.ToXmlString(true);
            publicKey = rsa.ToXmlString(false);
            return rsa;
        }
    }
}
