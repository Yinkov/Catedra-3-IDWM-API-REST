using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using apiCatedra3.src.interfaces;
using apiCatedra3.src.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace apiCatedra3.src.Services
{
    public class TokenService : ITokenService
    {
    
        private readonly SymmetricSecurityKey _key;
        private readonly UserManager<AppUser> _userManager;
         
        public TokenService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
           
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SigningKey")?? throw new ArgumentNullException("Jwt:SigningKey")));

        }

        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),

            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds,
                Issuer = Environment.GetEnvironmentVariable("JWT_Issuer"),
                Audience = Environment.GetEnvironmentVariable("JWT_Audience")
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}