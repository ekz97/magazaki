using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace PeasieLib.Services;

public static class TokenService
{
    private static readonly Guid _guid = Guid.NewGuid();

    public static async Task<string> GenerateToken()
    {
        var token = new JwtSecurityToken(new JwtHeader(), new JwtPayload());
        return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }

    public static async Task<string> GenerateSignedToken(string issuer, string audience, SymmetricSecurityKey symmetricKey)
    {
        var token = new JwtSecurityToken(issuer, audience, null, null, DateTime.Now.AddHours(1), new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha512));
        token.Payload.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }

    public static async Task<string> GenerateSignedTokenFromCertificate(string issuer, string audience, X509SecurityKey asymmetricKey)
    {
        var token = new JwtSecurityToken(issuer, audience, null, null, DateTime.Now.AddHours(1), new SigningCredentials(asymmetricKey, SecurityAlgorithms.RsaSha512));
        token.Payload.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }

    public static async Task<string> GenerateEncryptedToken(string issuer, string audience, SymmetricSecurityKey symmetricKey)
    {
        var token = new JwtSecurityTokenHandler().CreateJwtSecurityToken(issuer, audience, null, null, DateTime.Now.AddHours(1), DateTime.Now, new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha512), new EncryptingCredentials(symmetricKey, JwtConstants.DirectKeyUseAlg, SecurityAlgorithms.Aes256CbcHmacSha512));
        token.Payload.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }

    public static async Task<string> GenerateEncryptedTokenFromCertificate(string issuer, string audience, SymmetricSecurityKey symmetricKey, X509SecurityKey asymmetricKey)
    {
        var token = new JwtSecurityTokenHandler().CreateJwtSecurityToken(issuer, audience, null, null, DateTime.Now.AddHours(1), DateTime.Now, new SigningCredentials(asymmetricKey, SecurityAlgorithms.RsaSha512), new EncryptingCredentials(symmetricKey, JwtConstants.DirectKeyUseAlg, SecurityAlgorithms.Aes256CbcHmacSha512));
        token.Payload.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }

    public static async Task<string> GenerateJOSEFromCertificate(string issuer, string audience, X509SecurityKey asymmetricKey, X509SecurityKey encryptingCertificateKey)
    {
        var token = new JwtSecurityTokenHandler().CreateJwtSecurityToken(
            issuer: issuer, audience: audience, subject: new ClaimsIdentity(), notBefore: DateTime.Now, expires: DateTime.Now.AddMinutes(30), issuedAt: DateTime.Now,
            signingCredentials: new SigningCredentials(asymmetricKey, SecurityAlgorithms.RsaSsaPssSha512),
            encryptingCredentials: new EncryptingCredentials(encryptingCertificateKey, SecurityAlgorithms.RsaOAEP, SecurityAlgorithms.Aes256CbcHmacSha512));

        return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }

    public static async Task<string> GenerateEncryptedTokenNotSigned(string issuer, string audience, SymmetricSecurityKey symmetricKey, X509SecurityKey asymmetricKey)
    {
        var token = new JwtSecurityTokenHandler().CreateJwtSecurityToken(issuer: issuer, audience: audience, subject: null, notBefore: null, expires: null, issuedAt: null, signingCredentials: null, new EncryptingCredentials(symmetricKey, JwtConstants.DirectKeyUseAlg, SecurityAlgorithms.Aes256CbcHmacSha512));
        token.Payload.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }

    // https://www.scottbrady91.com/c-sharp/json-web-encryption-jwe-in-dotnet-core
    // https://www.scottbrady91.com/jose/json-web-encryption
    // https://www.scottbrady91.com/jose/jwts-which-signing-algorithm-should-i-use
    public static async Task<string> GenerateJOSERandomlySigned(string? payloadName = null, string? payload = null, int validityMinutes = 30)
    {
        GeneratePPKRandomly(out string privateKey, out string publicKey);

        var rsaCryptoServiceProvider = new RSACryptoServiceProvider(2048);
        rsaCryptoServiceProvider.FromXmlString(privateKey);
        var prk = rsaCryptoServiceProvider.ExportParameters(true);
        var puk = rsaCryptoServiceProvider.ExportParameters(false);

        var token = new JwtSecurityTokenHandler().CreateJwtSecurityToken(
            issuer: _guid.ToString(), // "Issuer", 
            audience: "Peasie", // "Audience",
            subject: new ClaimsIdentity(),
            notBefore: DateTime.Now,
            expires: DateTime.Now.AddMinutes(validityMinutes),
            issuedAt: DateTime.Now,
            signingCredentials: new SigningCredentials(new RsaSecurityKey(prk), SecurityAlgorithms.RsaSha256),
            encryptingCredentials: new EncryptingCredentials(new RsaSecurityKey(puk), SecurityAlgorithms.RsaOAEP, SecurityAlgorithms.Aes256CbcHmacSha512));

        if (!string.IsNullOrEmpty(payloadName) && !string.IsNullOrEmpty(payload))
        {
            token.Payload[payloadName] = payload;
        }

        return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }

    public static RSA GeneratePPKRandomly(out string privateKey, out string publicKey, int keySizeInBits = 2048)
    {
        var rsa = RSA.Create(keySizeInBits);
        privateKey = rsa.ToXmlString(true);
        publicKey = rsa.ToXmlString(false);
        return rsa;
    }
}