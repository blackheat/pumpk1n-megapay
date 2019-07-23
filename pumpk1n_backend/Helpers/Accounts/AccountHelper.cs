using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using pumpk1n_backend.Enumerations;
using pumpk1n_backend.Models.DatabaseContexts;
using pumpk1n_backend.Settings;

namespace pumpk1n_backend.Helpers.Accounts
{
    public class AccountHelper : IAccountHelper
    {
        private readonly DatabaseContext _context;
        private readonly JwtSettings _jwtSettings;

        public AccountHelper(DatabaseContext context, IOptions<JwtSettings> jwtSettings)
        {
            _context = context;
            _jwtSettings = jwtSettings.Value;
        }
        
        public String JwtGenerator(Int64 userId, Int64 loginAttemptId, UserType userType)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, userId.ToString(CultureInfo.InvariantCulture)),
                    new Claim(ClaimTypes.Sid, loginAttemptId.ToString(CultureInfo.InvariantCulture)),
                    new Claim(ClaimTypes.Role, userType.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(_jwtSettings.Expiry),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return jwt;
        }
    }
}