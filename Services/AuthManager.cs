using AutoMapper;
using HotelApi.Data;
using HotelApi.DTOS.WriteDtos;
using HotelApi.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HotelApi.Services
{
    public class AuthManager : IAuthManager
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApiUser> _userManager;

        public AuthManager(IConfiguration configuration, UserManager<ApiUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<bool> AuthenticateUserAsync(LoginWriteDto loginInfo)
        {
            var user = await _userManager.FindByNameAsync(loginInfo.UserName);
            if (user == null)
                return false;

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginInfo.Password);
            return isPasswordValid;
        }

        public async Task<string> GenerateTokenAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                return null;

            var signinCredentials = GetSigningCredentials();
            var claims = await GetClaims(user);

            var tokenOptions = GetTokenOptions(signinCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private JwtSecurityToken GetTokenOptions(SigningCredentials signinCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("jwt");
            var issuer = jwtSettings.GetSection("Issuer").Value;
            var lifeTime = Convert.ToDouble(jwtSettings.GetSection("LifeTime").Value);
            var expiration = DateTime.Now.AddHours(lifeTime);

            var token = new JwtSecurityToken(issuer: issuer, claims: claims, expires: expiration, signingCredentials: signinCredentials);

            return token;
        }

        private async Task<List<Claim>> GetClaims(ApiUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var jwtKey = Environment.GetEnvironmentVariable("KEY");
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
    }
}
