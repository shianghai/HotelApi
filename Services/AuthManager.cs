using AutoMapper;
using HotelApi.Data;
using HotelApi.DTOS.WriteDtos;
using HotelApi.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelApi.Services
{
    public class AuthManager : IAuthManager
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApiUser> _userManager;
        private ApiUser _user;

        public AuthManager(IConfiguration configuration, UserManager<ApiUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }


        public async Task<bool> AuthenticateUserAsync(LoginWriteDto loginInfo)
        {
            var _user  = await _userManager.FindByNameAsync(loginInfo.UserName);
            if (_user == null) return false;
            var isPasswordValid = await _userManager.CheckPasswordAsync(_user, loginInfo.Password);
            return isPasswordValid;
        }

        public async Task<string> GenerateTokenAsync()
        {
            var signinCredentials = GetSigningCredentials();
            var claims = GetClaims();
            var tokenOptions =await GetTokenOptions(signinCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private async Task<JwtSecurityToken> GetTokenOptions(SigningCredentials signinCredentials, Task<List<Claim>> claims)
        {
            var jwtSettings = _configuration.GetSection("jwt");
            var issuer = jwtSettings.GetSection("Issuer").Value;
            var lifeTime = Convert.ToDouble(jwtSettings.GetSection("LifeTime").Value);
            var expiration = DateTime.Now.AddHours(lifeTime);

            var tokenClaims = await claims;
            var token = new JwtSecurityToken(issuer: issuer, claims: tokenClaims, expires: expiration, signingCredentials: signinCredentials);

            return token;
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, _user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(_user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
            
        }

        private SigningCredentials GetSigningCredentials()
        {
            var jwtKey = Environment.GetEnvironmentVariable("JWTKEY");
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
    }
}
