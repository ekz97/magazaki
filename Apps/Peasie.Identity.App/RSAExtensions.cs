using System.Security.Cryptography;
using System.Text;

namespace Peasie.Web
{
    public static class RSAExtensions
    {
        private const string RsaPublickeyPemHeader = "-----BEGIN RSA PUBLIC KEY-----";
        private const string RsaPublickeyPemFooter = "-----END RSA PUBLIC KEY-----";
        private const string SubjectPublicKeyInfoPemHeader = "-----BEGIN PUBLIC KEY-----";
        private const string SubjectPublicKeyInfoPemFooter = "-----END PUBLIC KEY-----";

        private const string RsaOid = "1.2.840.113549.1.1.1";

        public static string ExportToPem(this RSA key, RsaPublicKeyFormat format)
        {
            var buffer = new StringBuilder();

            if (format == RsaPublicKeyFormat.RsaPublicKey)
            {
                buffer.AppendLine(RsaPublickeyPemHeader);
                buffer.AppendLine(Convert.ToBase64String(
                  key.ExportRSAPublicKey(),
                  Base64FormattingOptions.InsertLineBreaks));
                buffer.AppendLine(RsaPublickeyPemFooter);
            }
            else if (format == RsaPublicKeyFormat.SubjectPublicKeyInfo)
            {
                buffer.AppendLine(SubjectPublicKeyInfoPemHeader);
                buffer.AppendLine(Convert.ToBase64String(
                  key.ExportSubjectPublicKeyInfo(),
                  Base64FormattingOptions.InsertLineBreaks));
                buffer.AppendLine(SubjectPublicKeyInfoPemFooter);
            }
            else
            {
                throw new ArgumentException(nameof(format));
            }

            return buffer.ToString();
        }
    }
}