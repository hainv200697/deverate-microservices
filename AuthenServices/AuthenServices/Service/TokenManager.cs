using AuthenServices.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenServices.Service
{
    public class TokenManager
    {
        private static string Secret = "DeverateProjectByGroup7";
        public static string GenerateToken(Account account)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim> {
                new Claim("accountId",  account.AccountId.ToString()),
                new Claim("username", account.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, account.Role.RoleName),
                new Claim("fullname", account.Fullname),
                new Claim("companyId", account.CompanyId.ToString())};
             
            var token = new JwtSecurityToken(
                "http://localhost:59347",
                "http://localhost:59347",
                claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
