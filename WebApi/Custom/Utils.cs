﻿using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApi.Models.Context;

namespace WebApi.Custom
{
    public class Utils
    {
        private IConfiguration _configuration;
        public Utils(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string encriptarSHA256(string text)
        {
            using (SHA256 sha256 = SHA256.Create()) {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
                StringBuilder sb = new StringBuilder();
                for(int i =0; i < bytes.Length; i++)
                {
                    sb.Append(bytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
        public string generarJWT(TblUsuario user)
        {
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role, user.IdRol.ToString()!),
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var jwtConfig = new JwtSecurityToken(
                claims: userClaims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
        }
    }
}
