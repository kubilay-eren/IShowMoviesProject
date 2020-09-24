using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IShowMovies.Settings.AppSetting
{
    public class AppSettings
    {
        public LoginSettings LoginSettings { get; set; }

        public JWT JWT { get; set; }

        public TheMovieSettings movieSettings { get; set; }

        public QuartzSettings quartzSettings { get; set; }

        public MailSettings mailSettings { get; set; }
    }
}
