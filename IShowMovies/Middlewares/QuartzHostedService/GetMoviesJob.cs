using IShowMovies.Extensions;
using IShowMovies.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IShowMovies.Middlewares.QuartzHostedService
{
    public class GetMoviesJob: IJob
    {
        private readonly IServiceCollection _services;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetMoviesJob(IServiceCollection services, IHttpContextAccessor httpContextAccessor)
        {
            _services = services;
            _httpContextAccessor = httpContextAccessor;
        }

        async Task IJob.Execute(IJobExecutionContext context)
        {
            _httpContextAccessor.HttpContext.Session.ClearMovie();
            var sp = _services.BuildServiceProvider();
            var _movieService = sp.GetService<IMovieService>();
            await _movieService.UpdateMoviesList();
        }
    }
}
