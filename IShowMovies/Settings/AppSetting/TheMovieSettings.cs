using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IShowMovies.Settings.AppSetting
{
    public class TheMovieSettings
    {
        public string BaseUrl { get; set; }
        public string Token { get; set; }

        public int MovieCount { get; set; }
    }
}
