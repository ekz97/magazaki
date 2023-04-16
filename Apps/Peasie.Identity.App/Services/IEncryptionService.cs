using System.Security.Cryptography;

namespace Peasie.Web.Services
{
    public interface IEncryptionService
    {
        RSA GeneratePPKRandomly(out string privateKey, out string publicKey, int keySizeInBits = 2048);
    }
}
