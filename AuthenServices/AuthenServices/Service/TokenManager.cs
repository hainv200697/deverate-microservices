using AuthenServices.Model;
using AuthenServices.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthenServices.Service
{
    public class TokenManager
    {
        private static string Secret = "ERMN05OPLoDvbTTa/QkqLNMI7cPLguaRyHzyg7n5qNBVjQmtBhz4SzYh4NBVCXi3KJHlSXKP+oi2+bXr6CUYTR==";
        public static string GenerateToken(Account account)
        {
            byte[] key = Convert.FromBase64String(Secret);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            DateTime expires = DateTime.UtcNow.AddDays(7);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor

            {
                Subject = new ClaimsIdentity(new[]
                        {
                            new Claim("accountId",  account.AccountId.ToString()),
                            new Claim("username", account.Username),
                            new Claim(ClaimTypes.Role, account.Role.Description),
                            new Claim("fullname", account.Fullname),
                            new Claim("companyId", account.CompanyId.ToString())
                        }),
                Expires = expires,
                SigningCredentials = new SigningCredentials(securityKey,
                SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
            return handler.WriteToken(token);
        }
    }
}
