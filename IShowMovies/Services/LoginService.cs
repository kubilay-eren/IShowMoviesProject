using IShowMovies.Models.Authentication;
using IShowMovies.ServiceInterfaces;
using IShowMovies.Settings.AppSetting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IShowMovies.Services
{
    public class LoginService : ILoginService
    {
        private readonly IOptions<AppSettings> _appsettings;
        private readonly UserManager<ApplicationUser> userManager;

        public LoginService(IOptions<AppSettings> appsettings, UserManager<ApplicationUser> userManager)
        {
            _appsettings = appsettings;
            this.userManager = userManager;
        }

        public async Task<JwtSecurityToken> Login(LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.UserName);
            if (user != null && await CheckUserLoginAsync(model.UserName, model.Password))
            {
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appsettings.Value.JWT.Secret));

                var token = new JwtSecurityToken(
                    issuer: _appsettings.Value.JWT.ValidIssuer,
                    audience: _appsettings.Value.JWT.ValidAudience,
                    expires: DateTime.Now.AddDays(1),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return token;
            }
            throw new Exception("Kullanıcı Bilgileri Yanlış");
        }

        private async Task<bool> CheckUserLoginAsync(string UserName, string Password)
        {
            if (String.IsNullOrEmpty(UserName) || String.IsNullOrEmpty(Password))
                return false;
            else if (UserName != _appsettings.Value.LoginSettings.UserName || Password != _appsettings.Value.LoginSettings.Password)
                return false;

            return true;
        }
    }



}
