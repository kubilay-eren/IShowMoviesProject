using IShowMovies.Models.Authentication;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace IShowMovies.ServiceInterfaces
{
    public interface ILoginService
    {
        Task<JwtSecurityToken> Login(LoginModel login);
    }
}
