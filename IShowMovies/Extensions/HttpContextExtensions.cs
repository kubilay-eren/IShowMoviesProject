using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IShowMovies.Extensions
{
    public static class HttpContextExtensions
    {
        public static String GetUserName(this HttpContext Context)
        {
            return Context.User.FindFirst(ClaimTypes.Name)?.Value;
        }
    }
}
