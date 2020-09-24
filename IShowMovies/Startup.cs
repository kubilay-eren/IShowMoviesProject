using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IShowMovies.Middlewares.Extensions;
using IShowMovies.Middlewares.QuartzHostedService;
using IShowMovies.Models;
using IShowMovies.Models.Authentication;
using IShowMovies.ServiceInterfaces;
using IShowMovies.Services;
using IShowMovies.Settings.AppSetting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace IShowMovies
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services, IHttpContextAccessor httpContextAccessor)
        {
            #region AppSettings
            var appSettings = new AppSettings();

            Configuration.Bind(appSettings);
            services.Configure<AppSettings>(Configuration);
            #endregion

            services.AddDbContext<MovieContext>(opt =>
             opt.UseInMemoryDatabase("Moive"));

            services.AddControllers();

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            // Adding Jwt Bearer  
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.JWT.Secret)),
                };
            });

            #region Service Injections
            services.AddTransient<ILoginService, LoginService>();
            services.AddTransient<IMovieService, MovieService>();
            #endregion

            #region Quartz
            services.AddSingleton<IJobFactory, JobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddHostedService<QuartzHostedService>();

            services.AddSingleton(p => new GetMoviesJob(services, httpContextAccessor));

            services.AddSingleton(new JobSchedule(jobType: typeof(GetMoviesJob), cronExpression: "0 0 * ? * *", run: appSettings.quartzSettings == null ? false : appSettings.quartzSettings.GetMoviesJob)); //every hour "0 0 * ? * *
            #endregion


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("api", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Swagger on ASP:NET Core",
                    Description = "IShowMovies ASP.NET CORE API",
                    TermsOfService = new Uri("http://swagger.io/terms/"),
                    Contact = new OpenApiContact
                    {
                        Name = "Kubilay Eren Yüksel",
                        Email = "kubilayeren_@hotmail.com",
                    }
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseExceptionHandling();
            }
            else
            {
                app.UseExceptionHandling();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/api/swagger.json", "IShowMovies");
                c.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
