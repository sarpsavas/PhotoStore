using Application.Helpers.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;



namespace Application.Helpers.TokenGenerator
{
    public class AuthService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IConfiguration _configuration;

        public AuthService(JwtSettings jwtSettings,
            IConfiguration configuration)
        {
            _jwtSettings = jwtSettings;
            _configuration = configuration;

        }
        public string GenerateToken(string eMail)
        {
            var claims = new[]
            {
                
                new Claim(ClaimTypes.Email, eMail)
            };

            var keyString = _configuration["Jwt:Key"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString!));
            

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
