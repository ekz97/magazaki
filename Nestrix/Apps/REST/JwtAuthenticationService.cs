using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using dotenv.net;
using LogicLayer.Model;
using Microsoft.IdentityModel.Tokens;

namespace SecurityLayer;

public class JwtAuthenticationService
{
    private readonly JwtSecurityTokenHandler _tokenHandler;
    private readonly byte[] _signingKey;

    public JwtAuthenticationService()
    {
        DotEnv.Load();
        _tokenHandler = new JwtSecurityTokenHandler();
        //var secret = Environment.GetEnvironmentVariable("JWT_SIGNING_SECRET");
        //_signingKey = Encoding.UTF8.GetBytes(secret);
        _signingKey = Encoding.UTF8.GetBytes("4e245c98-e194-45ac-935d-316569ec6830");
    }

    public Task<string> GenerateJwtTokenAsync(Gebruiker gebruiker)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, gebruiker.Id.ToString()),
                // add any other necessary claims
            }),
            Expires = DateTime.UtcNow.AddSeconds(30),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_signingKey), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = _tokenHandler.CreateToken(tokenDescriptor);
        return Task.FromResult(_tokenHandler.WriteToken(token));
    }
}