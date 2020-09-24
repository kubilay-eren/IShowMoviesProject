using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IShowMovies.Middlewares.QuartzHostedService
{
    public class JobSchedule
    {
        public JobSchedule(Type jobType, string cronExpression, bool run)
        {
            JobType = jobType;
            CronExpression = cronExpression;
            Run = run;
        }

        public JobSchedule(Type jobType, string cronExpression)
        {
            JobType = jobType;
            CronExpression = cronExpression;
            Run = false;
        }

        public Type JobType { get; }
        public string CronExpression { get; }
        public bool Run { get; }
    }
}
