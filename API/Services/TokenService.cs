using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Services
{
    public class TokenService(IConfiguration config) : ITokenService
    {
        public string CreateToken(User user)
        {
            var tokenKey = config["TokenKey"] ?? throw new Exception("Cannot access tokenKey from appsettings");

            if (tokenKey.Length < 64) throw new Exception("Your token key needs to be longer than 64 characters");

            // This is like a shared password between the app and the user who is validating their token
            // It needs to be converted to this security key to be signed
            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(tokenKey));

            // A claim is a piece of information about the user. In this case, it stores the userId and their name
            List<Claim> claims =
            [
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.UserName)
            ];

            /** This describes the payload and metadata for the token you're about to create.
                Subject: The user’s claims (identity).
                Expires: When the token should expire (7 days here).
                SigningCredentials: How to sign the token — here using your key and HMAC SHA-512 algorithm. 
            */
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature)
            };

            JwtSecurityTokenHandler tokenHandler = new();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
