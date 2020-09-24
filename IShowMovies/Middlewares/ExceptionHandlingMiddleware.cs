using IShowMovies.Models.ResultModels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IShowMovies.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);
            }
            catch (Exception ex)
            {
                if (!httpContext.Response.HasStarted)
                {
                    Log.Error(ex, "Request Error");

                    httpContext.Response.StatusCode = 200;
                    httpContext.Response.ContentType = "application/json";
                    var response = new Result<String>();
                    response.SetError(ex);
                    var json = JsonConvert.SerializeObject(response);
                    await httpContext.Response.WriteAsync(json);
                }
            }
        }
    }
}
