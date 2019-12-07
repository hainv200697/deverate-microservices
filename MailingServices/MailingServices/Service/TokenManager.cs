using AuthenServices.Model;
using AuthenServices.Models;
using MailingServices.Models;
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
            string accountInfo = account.Username + "_" + account.RoleId + "_" + account.Fullname;
            byte[] key = Convert.FromBase64String(Secret);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            DateTime expires = DateTime.UtcNow.AddDays(7);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor

            {
                Subject = new ClaimsIdentity(new[] {
                      new Claim(ClaimTypes.Name, accountInfo)}),
                Expires = expires,
                SigningCredentials = new SigningCredentials(securityKey,
                SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
            return handler.WriteToken(token);
        }
        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if (jwtToken == null)
                    return null;
                byte[] key = Convert.FromBase64String(Secret);
                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
                SecurityToken securityToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token,
                      parameters, out securityToken);
                return principal;
            }
            catch
            {
                return null;
            }
        }
        public static string ValidateToken(string token)
        {
            string decodedInfo = null;
            ClaimsPrincipal principal = GetPrincipal(token);
            if (principal == null)
                return null;
            ClaimsIdentity identity = null;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (NullReferenceException)
            {
                return null;
            }
            Claim accountIfo = identity.FindFirst(ClaimTypes.Name);
            decodedInfo = accountIfo.Value;
            return decodedInfo;
        }

        public static AccountDTO CheckToken(string token)
        {
            var data = ValidateToken(token).Split('_');
            AccountDTO acc = new AccountDTO();
            acc.Username = data[0];
            acc.RoleId = int.Parse(data[1]);
            acc.Fullname = data[2];
            return acc;
        }
    }
}
