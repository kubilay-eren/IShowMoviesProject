using IShowMovies.Settings.AppSetting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IShowMovies.HtppProcess
{
    public class RestClientFactory
    {
        private String baseUrl;
        private readonly IOptions<AppSettings> _appsettings;
        public RestClientFactory(IOptions<AppSettings> appsettings)
        {
            _appsettings = appsettings;
            baseUrl = appsettings.Value.movieSettings.BaseUrl;
        }

        public HttpClient Create(String BaseUrl = "", int TimeOutSecond = 100)
        {

            if (!String.IsNullOrEmpty(BaseUrl))
                baseUrl = BaseUrl;

            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(baseUrl),
                Timeout = TimeSpan.FromSeconds(TimeOutSecond)
            };

            client.DefaultRequestHeaders.Add("Accept", "application/json");


            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _appsettings.Value.movieSettings.Token);

            return client;
        }
    }
}
