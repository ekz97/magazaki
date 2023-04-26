using System.Security.Cryptography;
using System.Text;

namespace PeasieLib.Services
{
    public class EncryptionService
    {
        public static string ToSHA256(string s)
        {
            byte[] hashValue = SHA256.HashData(Encoding.UTF8.GetBytes(s));
            return Convert.ToHexString(hashValue);
        }

        #region Symmetric
        private static string _EncryptSym(string data, out byte[] key)
        {
            byte[] initializationVector = Encoding.ASCII.GetBytes("abcede0123456789");
            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.GenerateKey();
                key = aes.Key;

                //aes.Key = Encoding.UTF8.GetBytes(key);

                aes.IV = initializationVector;
                var symmetricEncryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream as Stream, symmetricEncryptor, CryptoStreamMode.Write))
                    {
                        using (var streamWriter = new StreamWriter(cryptoStream as Stream))
                        {
                            streamWriter.Write(data);
                        }
                        return Convert.ToBase64String(memoryStream.ToArray());
                    }
                }
            }
        }

        private static string _DecryptSym(string cipherText, byte[] key)
        {
            byte[] initializationVector = Encoding.ASCII.GetBytes("abcede0123456789");
            byte[] buffer = Convert.FromBase64String(cipherText);
            using (Aes aes = Aes.Create())
            {
                //aes.KeySize = 256;
                aes.Key = key;
                aes.IV = initializationVector;
                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (var memoryStream = new MemoryStream(buffer))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream as Stream, decryptor, CryptoStreamMode.Read))
                    {
                        using (var streamReader = new StreamReader(cryptoStream as Stream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
        #endregion

        #region Asymmetric
        public static string Encrypt(string data, RSAParameters rsaParameters)
        {
            using (var rsaCryptoServiceProvider = new RSACryptoServiceProvider())
            {
                rsaCryptoServiceProvider.ImportParameters(rsaParameters);
                var byteData = Encoding.UTF8.GetBytes(data);
                var encryptedData = rsaCryptoServiceProvider.Encrypt(byteData, false);
                return Convert.ToBase64String(encryptedData);
            }
        }

        public static string EncryptUsingPublicKey(string data, string publicKeyXml /*, out byte[] symmetricPwd*/)
        {
            using (var rsaCryptoServiceProvider = new RSACryptoServiceProvider())
            {
                rsaCryptoServiceProvider.FromXmlString(publicKeyXml);
                var encryptedContent = _EncryptSym(data, out byte[] symmetricPwd);
                var pwdEncrypted = rsaCryptoServiceProvider.Encrypt(symmetricPwd, false);
                var pwdString = Convert.ToBase64String(pwdEncrypted);
                return pwdString.Length.ToString() + " " + pwdString + encryptedContent;
            }
        }

        public static string Decrypt(string cipherText, RSAParameters rsaParameters)
        {
            using (var rsaCryptoServiceProvider = new RSACryptoServiceProvider())
            {
                var cipherDataAsByte = Convert.FromBase64String(cipherText);
                rsaCryptoServiceProvider.ImportParameters(rsaParameters);
                var encryptedData = rsaCryptoServiceProvider.Decrypt(cipherDataAsByte, false);
                return Encoding.UTF8.GetString(encryptedData);
            }
        }

        public static string DecryptUsingPrivateKey(string cipherText, string privateKeyXml /*, out byte[] symmetricPwd*/)
        {
            var parts = cipherText.Split(" ");
            var length = int.Parse(parts[0]);
            var key = cipherText.Substring(parts[0].Length + 1, length);
            var data = cipherText.Substring(parts[0].Length + 1 + length);

            using (var rsaCryptoServiceProvider = new RSACryptoServiceProvider())
            {
                rsaCryptoServiceProvider.FromXmlString(privateKeyXml);
                byte[] symmetricPwd = Convert.FromBase64String(key);
                symmetricPwd = rsaCryptoServiceProvider.Decrypt(symmetricPwd, false);
                // Decrypt using actual key
                var decrypted = _DecryptSym(data, symmetricPwd);
                return decrypted;
            }
        }
        #endregion
    }
}
