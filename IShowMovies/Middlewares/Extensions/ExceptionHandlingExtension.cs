using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IShowMovies.Middlewares.Extensions
{
    public static class ExceptionHandlingExtension
    {
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));

            return app.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
